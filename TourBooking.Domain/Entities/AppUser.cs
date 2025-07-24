using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
  
    }
}