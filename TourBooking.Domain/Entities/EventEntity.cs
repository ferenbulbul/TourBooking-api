namespace TourBooking.Domain.Entities
{
    public class EventEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid? AvailabilityId { get; set; }
        public AvailabilityEntity Availability { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Title { get; set; }
        public string Start { get; set; }
        public bool AllDay { get; set; }
        public string Calendar { get; set; }
    }
}
