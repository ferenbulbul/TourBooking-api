using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourBooking.Domain.Entities
{
    public class GuideUserEntity : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Id))]
        public AppUser AppUser { get; set; }

        public string? LicenseNumber { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
