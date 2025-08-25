using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features;

public class DriverLocationsQueryResponse
{
    public IEnumerable<DriverLocationDto> Locations { get; set; } = new List<DriverLocationDto>();
}
