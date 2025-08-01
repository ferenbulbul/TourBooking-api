using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features;

public class VehicleQueryHandler : IRequestHandler<VehicleQuery, VehicleQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public VehicleQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<VehicleQueryResponse> Handle(
        VehicleQuery request,
        CancellationToken cancellationToken
    )
    {
        var vehicles = await _unitOfWork.Vehicles();

        if (vehicles == null || !vehicles.Any())
        {
            throw new NotFoundException("Araç bulunamadı.");
        }
        var dtos = vehicles.Select(tt => new VehicleDto
        {
            Id = tt.Id,
            AracResmi = tt.AracResmi,
            LegRoomSpaceId = tt.LegRoomSpaceId,
            LicensePlate = tt.LicensePlate,
            ModelYear = tt.ModelYear,
            Ruhsat = tt.Ruhsat,
            SeatCount = tt.SeatCount,
            SeatTypeId = tt.SeatTypeId,
            Sigorta = tt.Sigorta,
            Tasimacilik = tt.Tasimacilik,
            VehicleBrandId = tt.VehicleBrandId,
            VehicleClassId = tt.VehicleClassId,
            VehicleName = tt.VehicleName,
            VehicleTypeId = tt.VehicleTypeId,
            IsActive = tt.IsActive,
        });
        var response = new VehicleQueryResponse { Vehicles = dtos };
        return response;
    }
}
