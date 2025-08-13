using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Features.Mobile.Query.TourPointDetails;
using TourBooking.Application.Features.Settings;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class CreateBookingCommandHandler
    : IRequestHandler<CreateBookingCommand, CreateBookingCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateBookingCommandResponse> Handle(
        CreateBookingCommand request,
        CancellationToken cancellationToken
    )
    {
        
        var tourControl = await _unitOfWork.ControlTourRoute(request.TourPointId, request.CityId, request.DistrictId, request.VehicleId,0);
        if (tourControl == null)
        {
            throw new BusinessRuleValidationException("tur kontrol fayil");
        }
        var vehicleControl = await _unitOfWork.ControlVehicleAvalibity(request.VehicleId, request.Date);
        if (vehicleControl == null)
        {
            throw new BusinessRuleValidationException("araç kontrol fayil");
        }
        if (request.GuideId is not null)
        {
            var guideControl = await _unitOfWork.ControlGuideAvalibity(request.GuideId.Value, 0, request.Date, request.TourPointId, request.DistrictId, request.CityId);
            if (guideControl == null)
            {
                throw new BusinessRuleValidationException("rehberss kontrol fayil");
            }
        }
         using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            // 1️⃣ Booking ekle
            var bookingId = await _unitOfWork.FinishBooking(request);
            if (request.GuideId.HasValue)
            {
                await _unitOfWork.CreateGuideBlock(new CreateBlockCommand { GuideId = request.GuideId.Value, Start = request.Date, End = request.Date, Note = bookingId.ToString() });
            }

            await _unitOfWork.CreateVehicleBlock(request.VehicleId, request.Date);


            await _unitOfWork.CommitAsync();
            await transaction.CommitAsync();

            return new CreateBookingCommandResponse{BookingId = bookingId, IsValid=true};
        }
        catch
        {
            // Hata olursa rollback
            await transaction.RollbackAsync();
            throw new BusinessRuleValidationException("Rezervasyon oluşmadı ödemeyi bi ara alırsın");
        }
    }
}
