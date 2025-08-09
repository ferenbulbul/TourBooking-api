using System;

namespace TourBooking.Domain.Entities
{
    public class GuideTourPriceEntity
    {
        public Guid Id { get; set; }
        public Guid GuideId { get; set; }

        public Guid? CityId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? TourPointId { get; set; }

        public decimal Price { get; set; }
        public string Currency { get; set; } = "TRY";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
