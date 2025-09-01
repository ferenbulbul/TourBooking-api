using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features;

public class SystemCountsQueryResponse
{
    public IEnumerable<SystemCountDto> Counts { get; set; } = new List<SystemCountDto>();
}
