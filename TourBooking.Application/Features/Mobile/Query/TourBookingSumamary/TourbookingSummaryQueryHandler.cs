using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Features.Mobile.Query.TourPointDetails;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class TourbookingQueryHandler
    : IRequestHandler<TourBookingSummaryQuery, TourBookingSummaryQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public TourbookingQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TourBookingSummaryQueryResponse> Handle(
        TourBookingSummaryQuery request,
        CancellationToken cancellationToken
    )
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        // Adjust the parameters below to match the required signature of GetAllAsync
        var tourBookingSummaryVehicleTour = await _unitOfWork.TourBookingVehicleTourSummary(request.TourPointId,request.DistrictId,request.VehicleId,request.Date);
        var tourBookingSummaryGuide = await _unitOfWork.TourBookingGuideSummary(request.GuideId,request.DistrictId,request.TourPointId,request.Date);
        if (tourBookingSummaryVehicleTour == null && tourBookingSummaryGuide == null)
        {
            return new TourBookingSummaryQueryResponse();
        }
        

        var result= new MobileTourBookingSummaryDto
        {
            TourPointTitle = tourBookingSummaryVehicleTour.TourPointTitle,
            TourPointCity = tourBookingSummaryVehicleTour.TourPointCity,
            TourPointDistrict = tourBookingSummaryVehicleTour.TourPointDistrict,
            TourPrice = tourBookingSummaryVehicleTour.TourPrice,
            CarBrand = tourBookingSummaryVehicleTour.CarBrand,
            DriverName = tourBookingSummaryVehicleTour.DriverName,
            GuidName = tourBookingSummaryGuide.GuidName,
            GuidePrice = tourBookingSummaryGuide.GuidePrice,
            TotalPrice=tourBookingSummaryVehicleTour.TourPrice+tourBookingSummaryGuide.GuidePrice
        };

        var response = new TourBookingSummaryQueryResponse { TourBookingSummaryDto =  result };
        return response;
    }
}
