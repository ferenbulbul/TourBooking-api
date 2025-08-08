using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class MobileSearchVehiclesQueryHandler
    : IRequestHandler<MobileSearchVehiclesQuery, MobileSearchVehiclesQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public MobileSearchVehiclesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MobileSearchVehiclesQueryResponse> Handle(
        MobileSearchVehiclesQuery request,
        CancellationToken cancellationToken
    )
    {
        var vehicles = await _unitOfWork.MobileSearchVehicles(request);

        if (vehicles == null || !vehicles.Any())
        {
            return new MobileSearchVehiclesQueryResponse();
        }

        var response = new MobileSearchVehiclesQueryResponse { Vehicles = vehicles };
        return response;
    }
}
