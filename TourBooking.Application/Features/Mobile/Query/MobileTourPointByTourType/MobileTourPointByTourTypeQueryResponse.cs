using System;
using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features
{
    public class MobileTourPointByTourTypeQueryResponse
    {
        public IEnumerable<MobileDetailedSearchResultDto> TourPoints { get; set; }
    }
}
