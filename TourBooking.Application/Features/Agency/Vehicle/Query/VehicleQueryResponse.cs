using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features;

public class VehicleQueryResponse
{
    public IEnumerable<VehicleDto> Vehicles { get; set; }
}