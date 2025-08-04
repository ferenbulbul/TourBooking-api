using MediatR;

namespace TourBooking.Application.Features
{
    public class UpsertDriverCommand : IRequest
    {
        public Guid AgencyId { get; set; }
        public Guid? Id { get; set; }
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
