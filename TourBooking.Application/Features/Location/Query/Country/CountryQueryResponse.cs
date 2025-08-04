using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class CountryQueryResponse
    {
        public IEnumerable<CountryDto> Countries { get; set; }
    }
}
