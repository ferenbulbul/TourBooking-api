using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features;

public class MobileSearchGuideQueryResponse
{
    public IEnumerable<MobileSearchGuidesDto> Guides { get; set; }
}
