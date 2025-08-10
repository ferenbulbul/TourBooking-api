using System;

namespace TourBooking.Domain.Entities
{
    // Domain/Entities/TourRoutePriceEntity.cs
    public class TourRoutePriceEntity : IBaseEntity
    {
        public Guid Id { get; set; }

        public Guid AgencyId { get; set; } // hedef tur noktası
        public Guid TourPointId { get; set; } // hedef tur noktası

        public Guid? CountryId { get; set; }
        public Guid? RegionId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? DistrictId { get; set; }

        public Guid? VehicleId { get; set; }
        public Guid? DriverId { get; set; }

        public decimal Price { get; set; }
        public string Currency { get; set; } = "TRY";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
