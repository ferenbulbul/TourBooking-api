using System;
using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features
{
    public class TourBookingSummaryQueryResponse
    { 
        public MobileTourBookingSummaryDto TourBookingSummaryDto { get; set; } = new MobileTourBookingSummaryDto();
    }
}
