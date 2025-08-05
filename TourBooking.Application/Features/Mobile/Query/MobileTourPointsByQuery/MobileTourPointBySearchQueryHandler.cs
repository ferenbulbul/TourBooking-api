using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class MobileTourPointBySearchQueryHandler
    : IRequestHandler<MobileTourPointBySearchQuery, MobileTourPointBySearchQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public MobileTourPointBySearchQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MobileTourPointBySearchQueryResponse> Handle(
        MobileTourPointBySearchQuery request,
        CancellationToken cancellationToken
    )
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        // Adjust the parameters below to match the required signature of GetAllAsync
        var points = await _unitOfWork.MobileTourPointBySearch(request.Query, culture);

        if (points == null || !points.Any())
        {
            return new MobileTourPointBySearchQueryResponse();
        }

        return new MobileTourPointBySearchQueryResponse { TourPoints = points };
    }
}
