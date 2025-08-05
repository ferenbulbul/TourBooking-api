using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features;

public class MobileTourPointByTourTypeQueryResponse
{
    public IEnumerable<MobileTourPointDto> TourPoints { get; set; }
}
