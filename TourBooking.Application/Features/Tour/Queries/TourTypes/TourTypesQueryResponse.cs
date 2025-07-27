using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features;

public class TourTypesQueryResponse
{
    public IEnumerable<TourTypeDto> TourTypeDtos { get; set; }
}