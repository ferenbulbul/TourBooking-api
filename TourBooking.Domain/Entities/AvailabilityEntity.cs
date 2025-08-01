namespace TourBooking.Domain.Entities
{
    public class AvailabilityEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public VehicleEntity Vehicle { get; set; }
        public Guid VehicleId { get; set; }
        public ICollection<BusyDayEntity> BusyDays { get; set; } = new List<BusyDayEntity>();
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
