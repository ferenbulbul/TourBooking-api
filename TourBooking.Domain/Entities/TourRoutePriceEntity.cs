using System;

namespace TourBooking.Domain.Entities
{
    // Domain/Entities/TourRoutePriceEntity.cs
    public class TourRoutePriceEntity : IBaseEntity
    {
        public Guid Id { get; set; }

        public Guid AgencyId { get; set; }
        public AgencyUserEntity Agency { get; set; }

        public Guid TourPointId { get; set; } 
        public TourPointEntity TourPoint { get; set; } 

        public Guid CityId { get; set; }
        public CityEntity City { get; set; }         
         // (varsa)
        
        public Guid CountryId { get; set; }
        public CountryEntity Country { get; set; }


        public Guid RegionId { get; set; }
        public RegionEntity Region { get; set; }

         public Guid DistrictId { get; set; }
        public DistrictEntity District { get; set; }


        public Guid TourId { get; set; }
        public TourEntity Tour { get; set; }
        
        public Guid VehicleId { get; set; }
        public VehicleEntity Vehicle { get; set; }


        public Guid DriverId { get; set; }
        public DriverEntity Driver { get; set; }

        public decimal Price { get; set; }
        public string Currency { get; set; } = "TRY";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
