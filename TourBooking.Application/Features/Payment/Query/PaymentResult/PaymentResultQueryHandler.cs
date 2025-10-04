using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features.Payment.Query.PaymentResult
{
    public class PaymentResultQueryHandler : IRequestHandler<PaymentResultQuery, PaymentResultQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly INetgsmSmsService _smsService;

        public PaymentResultQueryHandler(IUnitOfWork unitOfWork, IEmailService emailService, INetgsmSmsService smsService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _smsService = smsService;
        }

        public async Task<PaymentResultQueryResponse> Handle(
            PaymentResultQuery request,
            CancellationToken cancellationToken
        )
        {
            var payment = await _unitOfWork.GetPaymentByTokenAsync(request.Token);
            if (payment.Status == PaymentStatus.Success)
            {
                var body = BuildPaymentSuccessEmail(payment.ConversationId, payment.Amount, "tl");
                await _emailService.SendEmailAsync(payment.Booking.Customer.AppUser.Email,
                    $"Siparişiniz Onaylandı – ", body);

                // Admin’e
                await _emailService.SendEmailAsync("ferenbulbul@gmail.com",
                    $"Yeni Sipariş Onaylandı – {payment.ConversationId}",
                    body);

                // Acenta’ya
                await _emailService.SendEmailAsync(payment.Booking.TourRoutePrice.Agency.AppUser.Email,
                    $"Size Bağlı Rezervasyon Onaylandı – {payment.ConversationId}",
                    body);


                var smSbody = $"Siparişiniz başarıyla onaylandı ✅\n" +
                        $"Rezervasyon No: {payment.ConversationId}\n" +
                        $"Tutar: {payment.Amount} TL";

                var messages = new List<SmsMessageDto>
                {
                    new SmsMessageDto(payment.Booking.Customer.AppUser.PhoneNumber, smSbody),                 // kullanıcı
                    new SmsMessageDto("05415704552", smSbody),                                              // admin
                    new SmsMessageDto(payment.Booking.TourRoutePrice.Agency.AppUser.PhoneNumber, smSbody)                  // acenta
                };
                await _smsService.SendBatchAsync(messages);


            }
            var response = new PaymentResultQueryResponse
            {
                ConversationId = payment.ConversationId,
                PaymentStatus = payment.Status.ToString(),
            };
            return response;
        }
        private string BuildPaymentSuccessEmail(string conversationId, decimal paidPrice, string currency)
        {
            return $@"
                <!DOCTYPE html>
                <html lang=""tr"">
                <head>
                <meta charset=""UTF-8"">
                <style>
                    body {{ font-family: Arial, sans-serif; color: #333; }}
                    .container {{ max-width: 600px; margin: auto; border: 1px solid #eee; padding: 20px; border-radius: 8px; }}
                    .header {{ text-align: center; padding-bottom: 10px; }}
                    .header h2 {{ color: #2d6a4f; }}
                    .details {{ margin-top: 20px; }}
                    .details p {{ line-height: 1.6; }}
                    .footer {{ margin-top: 30px; font-size: 13px; color: #777; text-align: center; }}
                    .highlight {{ font-weight: bold; color: #2d6a4f; }}
                </style>
                </head>
                <body>
                <div class=""container"">
                    <div class=""header"">
                    <h2>🎉 Siparişiniz Başarıyla Onaylandı!</h2>
                    </div>

                    <p>Merhaba,</p>
                    <p>Rezervasyonunuz başarıyla oluşturulmuş ve ödeme işleminiz onaylanmıştır.</p>

                    <div class=""details"">
                    <p><span class=""highlight"">Sipariş Numarası:</span> {conversationId}</p>
                    <p><span class=""highlight"">Ödeme Tutarı:</span> {paidPrice} {currency}</p>
                    <p><span class=""highlight"">Durum:</span> Onaylandı ✅</p>
                    </div>

                    <p>Rezervasyon detaylarını hesabınızdan görüntüleyebilirsiniz.  
                    Sorularınız olursa bizimle iletişime geçebilirsiniz.</p>

                    <div class=""footer"">
                    <p>Teşekkürler,<br/>TourBooking Ekibi</p>
                    </div>
                </div>
                </body>
                </html>";
        }

    }



}


