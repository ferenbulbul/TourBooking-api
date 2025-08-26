using Microsoft.AspNetCore.Identity;
using TourBooking.Domain.Enums;

namespace TourBooking.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public UserType UserType { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireDate { get; set; }
        public CustomerUser CustomerUser { get; set; }
        public AgencyUserEntity AgencyUser { get; set; }
        public GuideUserEntity GuideUser { get; set; }
        public CallCenterAgentEntity CallCenterAgent { get; set; }
        public bool? IsFirstLogin { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.Now;
    }
}
