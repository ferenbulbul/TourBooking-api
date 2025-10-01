using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features.Payment.Command.PaymentCallback
{
    public class PaymentCallbackCommmandHandler : IRequestHandler<PaymentCallbackCommand,PaymentCallbackCommandResponse>
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

            // DB update (Payment & Booking)
            Console.WriteLine(request.Token);
            Console.WriteLine("dto dönen token" +dto.Token);
            var payment = await _unitOfWork.GetPaymentByTokenAsync(dto.Token);
            if (payment != null)
            {
                payment.Status = dto.PaymentStatus == "SUCCESS"
                    ? PaymentStatus.Success
                    : PaymentStatus.Fail;

                payment.RawResponse = JsonSerializer.Serialize(dto);
                payment.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.GetRepository<PaymentEntity>().UpdateAsync(payment);


                var booking = await _unitOfWork.GetRepository<BookingEntity>().GetByIdAsync(payment.BookingId);
                if (booking != null)
                {
                    booking.Status = payment.Status == PaymentStatus.Success
                        ? BookingStatus.Confirmed
                        : BookingStatus.Cancelled;

                    booking.UpdatedDate = DateTime.UtcNow;
                    await _unitOfWork.GetRepository<BookingEntity>().UpdateAsync(booking);
                }

                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw   new BusinessRuleValidationException("Token ile kayıt bulunamadı");
            }

            return new PaymentCallbackCommandResponse{paymentResultDto=dto};
    }
}

}