using System;

namespace TourBooking.Domain.Entities
{
    public class GuideLanguageEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public GuideUserEntity Guide { get; set; }
        public Guid GuideId { get; set; }
        public Guid LanguageId { get; set; }
        public LanguageEntity Language { get; set; } = null!;
    }
}
