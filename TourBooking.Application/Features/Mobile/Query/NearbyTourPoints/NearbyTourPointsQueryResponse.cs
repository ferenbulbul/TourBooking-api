using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features;

public class NearbyTourPointsQueryResponse
{
    public IEnumerable<NearbyTourPointDto> NearByList { get; set; } 
}
