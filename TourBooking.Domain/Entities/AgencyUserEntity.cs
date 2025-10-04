using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourBooking.Domain.Entities
{
    public class AgencyUserEntity : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Id))]
        public AppUser AppUser { get; set; }= null!;
        public string AuthorizedUserFirstName { get; set; } = null!;
        public string AuthorizedUserLastName { get; set; }= null!;
        public string FullAddress { get; set; }= null!;
        public string City { get; set; }= null!;
        public string Country { get; set; }= null!;
        public string CompanyName { get; set; }= null!;
        public string Email { get; set; }= null!;
        public string? PhoneNumber2 { get; set; }= null!;
        public string PhoneNumber { get; set; }= null!;
        public string TaxNumber { get; set; }= null!;
        public string TursabUrl { get; set; }= null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsConfirmed { get; set; }
        public virtual ICollection<DriverEntity> Drivers { get; set; } = new List<DriverEntity>();
        public virtual ICollection<VehicleEntity> Vehicles { get; set; } = new List<VehicleEntity>();
        public virtual ICollection<TourEntity> Tours { get; set; } = new List<TourEntity>();

    }
}
