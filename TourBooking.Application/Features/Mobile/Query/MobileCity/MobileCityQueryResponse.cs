using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features;

public class MobileCityQueryResponse
{
    public IEnumerable<MobileCityDto> CityList { get; set; }
}