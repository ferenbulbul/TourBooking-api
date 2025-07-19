using System;

namespace TourBooking.Domain.Entities
{
    public class LanguageEntity : IBaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
