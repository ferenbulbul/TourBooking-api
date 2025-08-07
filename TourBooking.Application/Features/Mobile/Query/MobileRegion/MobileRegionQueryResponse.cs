using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features;

public class MobileRegionQueryResponse
{
    public IEnumerable<MobileRegionDto> RegionList { get; set; }
}