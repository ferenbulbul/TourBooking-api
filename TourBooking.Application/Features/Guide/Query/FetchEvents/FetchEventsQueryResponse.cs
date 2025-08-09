using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.GuideCalendar;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class FetchEventsQueryResponse
    {
        public IEnumerable<CalendarEventDto2> Events { get; set; }
    }
}
