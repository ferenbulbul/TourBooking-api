using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features;

public class MobileHighlightedTourPointQueryResponse
{
    public IEnumerable<MobileHighlightedTourPointDto> TourPoints { get; set; }
}
