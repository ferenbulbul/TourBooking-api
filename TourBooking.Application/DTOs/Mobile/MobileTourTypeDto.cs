using System;

namespace TourBooking.Application.DTOs.Mobile
{
    public class MobileTourTypeDto
    {
        public Guid Id { get; set; }
        public string MainImageUrl { get; set; }
        public string ThumbImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
