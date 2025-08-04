using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourBooking.Domain.Entities
{
    public class AgencyUserEntity : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        
        [ForeignKey(nameof(Id))]
        public AppUser AppUser { get; set; }
        public string? AuthorizedUserFirstName { get; set; }
        public string? AuthorizedUserLastName { get; set; }
        public string? FullAddress { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber2 { get; set; }
        public string? PhoneNumber { get; set; }
        public string? TaxNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsConfirmed { get; set; }
        public virtual ICollection<DriverEntity> Drivers { get; set; } = new List<DriverEntity>();
        public virtual ICollection<VehicleEntity> Vehicles { get; set; } = new List<VehicleEntity>();
        public virtual ICollection<TourEntity> Tours { get; set; } = new List<TourEntity>();
    }
}
