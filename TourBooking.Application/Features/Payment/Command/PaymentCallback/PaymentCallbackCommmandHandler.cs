using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Expactions;
using TourBooking.Application.Features.Settings;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features.Payment.Command.PaymentCallback
{
    public class PaymentCallbackCommmandHandler : IRequestHandler<PaymentCallbackCommand, PaymentCallbackCommandResponse>
    {
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentCallbackCommmandHandler(IPaymentService paymentService, IUnitOfWork unitOfWork)
        {
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentCallbackCommandResponse> Handle(PaymentCallbackCommand request, CancellationToken cancellationToken)
        {
            var dto = await _paymentService.RetrieveCheckoutFormAsync(request.Token);

            var payment = await _unitOfWork.GetPaymentByTokenAsync(dto.Token);
            if (payment != null)
            {
                payment.Status = dto.PaymentStatus == "SUCCESS"
                    ? PaymentStatus.Success
                    : PaymentStatus.Fail;

                payment.RetrieveRawResponse = dto.RetrieveRawResponse;
                payment.UpdatedDate = DateTime.UtcNow;


                await _unitOfWork.GetRepository<PaymentEntity>().UpdateAsync(payment);


                var booking = await _unitOfWork.GetRepository<BookingEntity>().GetByIdAsync(payment.BookingId);
                if (booking == null)
                {
                    throw new BusinessRuleValidationException("rezervasyon bulunamadı");
                }

                if (payment.Status == PaymentStatus.Success)
                {

                    using var transaction = await _unitOfWork.BeginTransactionAsync();
                    try
                    {
                        await _unitOfWork.CreateSystemVehicleBlock(new CreateVehicleBlockCommand { VehicleId = booking!.VehicleId, Start = booking.StartDate, End = booking.EndDate, Note = booking.Id.ToString() });
                        if (booking.GuideId.HasValue)
                        {
                            await _unitOfWork.CreateSystemGuideBlock(new CreateBlockCommand { GuideId = booking.GuideId.Value, Start = booking.StartDate, End = booking.EndDate, Note = booking.Id.ToString() });
                        }
                        booking.Status = BookingStatus.Success;
                        booking.UpdatedDate = DateTime.UtcNow;
                        
                        await _unitOfWork.GetRepository<BookingEntity>().UpdateAsync(booking);

                        await _unitOfWork.CommitAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine("Rollback çalıştı" + ex);
                        // 📌 Logla: Payment Success ama block eklenemedi
                        // İleride manuel müdahale gerekebilir
                    }
                }
            }
            else
            {
                throw new BusinessRuleValidationException("Token ile kayıt bulunamadı");
            }
            var response = new PaymentCallbackCommandResponse
            {
                ConversationId = dto.ConversationId,
                Token = dto.Token,
                PaymentStatus = dto.PaymentStatus,
                PaymentId = dto.PaymentId,
                PaidPrice = dto.PaidPrice,
                Currency = dto.Currency
            };
            return response;
        }
    }

}