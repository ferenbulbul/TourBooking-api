using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features;

public class TourPointsQueryResponse
{
    public IEnumerable<TourPointDto> TourPoints { get; set; }
}