using System;
using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features
{
    public class MobileDetailedSearchQueryResponse
    {
        public IEnumerable<MobileDetailedSearchResultDto> TourPoints { get; set; }
    }
}
