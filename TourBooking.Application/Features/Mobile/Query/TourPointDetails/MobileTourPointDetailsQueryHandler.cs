using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Features.Mobile.Query.TourPointDetails;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class MobileTourPointDetailsQueryHandler
    : IRequestHandler<MobileTourPointDetailsQuery, MobileTourPointDetailsQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public MobileTourPointDetailsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MobileTourPointDetailsQueryResponse> Handle(
        MobileTourPointDetailsQuery request,
        CancellationToken cancellationToken
    )
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        // Adjust the parameters below to match the required signature of GetAllAsync
        var tpDetails = await _unitOfWork.MobileTourPointDetail(request.TourPointId, culture);

        if (tpDetails == null)
        {
            return new MobileTourPointDetailsQueryResponse();
        }
        var response = new MobileTourPointDetailsQueryResponse { TourPointDetails = tpDetails };
        return response;
    }
}
