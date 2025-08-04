using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features;

public class MobileTourTypesQueryResponse
{
    public IEnumerable<MobileTourTypeDto> TourTypes { get; set; }
}