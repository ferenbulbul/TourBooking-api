using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features;

public class CustomerFavoritesQueryResponse
{
    public IEnumerable<MobileHighlightedTourPointDto> TourPoints { get; set; } 
}
