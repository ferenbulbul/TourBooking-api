using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class MobileDistrictQueryHandler
    : IRequestHandler<MobileDistrictQuery, MobileDistrictQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public MobileDistrictQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MobileDistrictQueryResponse> Handle(
        MobileDistrictQuery request,
        CancellationToken cancellationToken
    )
    {
        var culture = CultureInfo.CurrentUICulture.Name;

        var districts = await _unitOfWork.DistrictsForMobile(request.CityId, culture);

        if (districts == null || !districts.Any())
        {
            return new MobileDistrictQueryResponse();
        }

        var response = new MobileDistrictQueryResponse { DistrictList = districts };
        return response;
    }
}
