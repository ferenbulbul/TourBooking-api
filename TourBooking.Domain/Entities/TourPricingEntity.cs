namespace TourBooking.Domain.Entities
{
    public class TourPricingEntity : IBaseEntity
    {
        public bool IsActive { get; set; }
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public CountryEntity Country { get; set; }
        public Guid CityId { get; set; }
        public CityEntity City { get; set; }
        public Guid DistrictId { get; set; }
        public DistrictEntity District { get; set; }
        public Guid RegionId { get; set; }
        public RegionEntity Region { get; set; }
        public Guid VehicleId { get; set; }
        public VehicleEntity Vehicle { get; set; }
        public Guid DriverId { get; set; }
        public DriverEntity Driver { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid TourId { get; set; }
        public TourEntity Tour { get; set; }
        public decimal Price { get; set; }
    }
}
