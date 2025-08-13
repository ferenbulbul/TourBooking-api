using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Features.Mobile.Query.TourPointDetails;
using TourBooking.Application.Features.Settings;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features;

public class FinishBookingCommandHandler
    : IRequestHandler<FinishBookingCommand, FinishBookingCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public FinishBookingCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<FinishBookingCommandResponse> Handle(
        FinishBookingCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingBooking = await _unitOfWork.GetRepository<BookingEntity>().GetByIdAsync(request.BookingId);
        if (existingBooking == null)
        {
            throw new BusinessRuleValidationException("");
        }
        //sanal pos call başarılı ise alta devamke
        existingBooking.Status = BookingStatus.Confirmed;
        await _unitOfWork.GetRepository<BookingEntity>().UpdateAsync(existingBooking);

        return new FinishBookingCommandResponse { BookingId = existingBooking.Id };
        // booking tablosuna insert 
        // rehber gün iptali
        // araç gün iptali
        // acenta mail + sms 
        // müşteri mail + sms 
        // müşteriye booking id dönüşü 
        // e fatura api call 


    }
}
