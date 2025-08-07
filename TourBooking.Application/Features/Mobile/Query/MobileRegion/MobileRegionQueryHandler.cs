using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class MobileRegionQueryHandler : IRequestHandler<MobileRegionQuery, MobileRegionQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public MobileRegionQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MobileRegionQueryResponse> Handle(
        MobileRegionQuery request,
        CancellationToken cancellationToken
    )
    {
        var culture = CultureInfo.CurrentUICulture.Name;

        var regions = await _unitOfWork.RegionsForMobile(culture);

        if (regions == null || !regions.Any())
        {
            return new MobileRegionQueryResponse();
        }

        var response = new MobileRegionQueryResponse { RegionList = regions };
        return response;
    }
}
