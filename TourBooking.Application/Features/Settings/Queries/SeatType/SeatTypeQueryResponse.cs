using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class SeatTypeQueryResponse
    {
        public IEnumerable<SeatTypeDto> SeatTypes { get; set; }
    }
}
