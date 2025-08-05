using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features;

public class MobileDistrictQueryResponse
{
    public IEnumerable<MobileDistrictDto> DistrictList { get; set; }
}
