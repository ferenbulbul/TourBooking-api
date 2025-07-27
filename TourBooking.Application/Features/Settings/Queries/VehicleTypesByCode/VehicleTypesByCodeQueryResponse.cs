using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Queries;

public class VehicleTypesByCodeQueryResponse
{
    public IEnumerable<VehicleTypeDto> VehicleTypes { get; set; }
}