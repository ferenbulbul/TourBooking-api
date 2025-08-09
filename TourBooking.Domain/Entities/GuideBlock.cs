using System;

namespace TourBooking.Domain.Entities
{
    public class GuideBlock : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid GuideId { get; set; }
        public GuideUserEntity Guide { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
