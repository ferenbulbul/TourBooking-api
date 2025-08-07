using System;
using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features.Mobile.Query.TourPointDetails
{
    public class MobileTourPointDetailsQueryResponse
    { 
        public MobileTourPointDetailDto TourPointDetails { get; set; } = new MobileTourPointDetailDto();
    }
}
