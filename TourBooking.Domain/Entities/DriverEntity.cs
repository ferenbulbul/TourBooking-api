using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
    public class DriverEntity : IBaseEntity
    {
        public Guid Id { get; set; }
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
    }
}
