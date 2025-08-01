using System;

namespace TourBooking.Domain.Entities
{
    public class TourEntity : IBaseEntity
    {
        public bool IsActive { get; set; }
        public Guid Id { get; set; }
        public TourPointEntity TourPoint { get; set; }
        public Guid TourPointId { get; set; }
        public ICollection<TourPricingEntity> PricingEntity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
