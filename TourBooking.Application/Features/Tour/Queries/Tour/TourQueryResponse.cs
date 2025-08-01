using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features;

public class TourQueryResponse
{
    public IEnumerable<TourDto> Tours { get; set; }
}