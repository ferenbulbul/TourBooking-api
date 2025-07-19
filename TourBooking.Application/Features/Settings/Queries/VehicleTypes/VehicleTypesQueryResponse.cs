using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Queries;

public class VehicleTypesQueryResponse
{
    public IEnumerable<VehicleTypeDto> VehicleTypes { get; set; }
}