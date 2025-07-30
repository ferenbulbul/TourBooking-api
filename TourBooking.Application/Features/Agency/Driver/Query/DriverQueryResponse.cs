using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features;

public class DriverQueryResponse
{
    public IEnumerable<DriverDto> Drivers { get; set; }
}