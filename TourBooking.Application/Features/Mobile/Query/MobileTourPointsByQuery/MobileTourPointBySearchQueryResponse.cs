using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features;

public class MobileTourPointBySearchQueryResponse
{
    public IEnumerable<MobileTourPointsBySearchDto> TourPoints { get; set; }
}
