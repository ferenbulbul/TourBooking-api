using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class NearbyTourPointsQueryHandler
    : IRequestHandler<NearbyTourPointsQuery, NearbyTourPointsQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public NearbyTourPointsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NearbyTourPointsQueryResponse> Handle(
        NearbyTourPointsQuery request,
        CancellationToken cancellationToken
    )
    {
        var points = await _unitOfWork.NearbyTourPoints(request.CustomerId);
        if (points == null || !points.Any())
        {
            return new NearbyTourPointsQueryResponse();
        }
        var response = new NearbyTourPointsQueryResponse { NearByList = points };
        return response;
    }
}
