using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TourBooking.Domain.Entities;

namespace TourBooking.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<EmailVerificationCode> EmailVerificationCodes { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<VehicleBrand> VehicleBrands { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Fluent API konfigürasyonları yapılacaksa buraya
        }
    }
}
