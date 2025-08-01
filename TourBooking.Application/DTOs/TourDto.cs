using System;

namespace TourBooking.Application.DTOs
{
    public class TourDto
    {
        public Guid Id { get; set; }
        public Guid TourPointId { get; set; }
        public string TourPointName { get; set; }
        public IEnumerable<PricingDto> Pricings { get; set; }
    }
}
