using System.ComponentModel.DataAnnotations.Schema;

namespace TourBooking.Domain.Entities
{
    public class DriverEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        [ForeignKey(nameof(Id))]
        public virtual AppUser AppUser { get; set; }
        public Guid AgencyId { get; set; }
        public AgencyUserEntity Agency { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string DateOfBirth { get; set; }
        public string DriversLicenceDocument { get; set; }
        public string ExperienceYears { get; set; }
        public string IdentityNumber { get; set; }
        public bool IsActive { get; set; }
        public List<string> LanguagesSpoken { get; set; }
        public string NameSurname { get; set; }
        public string ProfilePhoto { get; set; }
        public List<string> ServiceCities { get; set; }
        public string? SrcDocument { get; set; }
        public string? PsikoDocument { get; set; }
        public ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
        public DriverLocationEntity DriverLocation { get; set; }
    }
}
