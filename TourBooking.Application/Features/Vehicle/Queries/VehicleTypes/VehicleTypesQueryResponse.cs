using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Vehicles.Queries.Vehicles;

public class VehicleTypesQueryResponse
{
    public IEnumerable<VehicleTypeDto> VehicleTypes { get; set; }
}