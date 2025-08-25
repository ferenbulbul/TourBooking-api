using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class DriverLocationsQueryHandler
    : IRequestHandler<DriverLocationsQuery, DriverLocationsQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DriverLocationsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DriverLocationsQueryResponse> Handle(
        DriverLocationsQuery request,
        CancellationToken cancellationToken
    )
    {
        var resultList = await _unitOfWork.DriverLocations();
        if (resultList == null)
        {
            return new DriverLocationsQueryResponse();
        }
        return new DriverLocationsQueryResponse { Locations = resultList };
    }
}
