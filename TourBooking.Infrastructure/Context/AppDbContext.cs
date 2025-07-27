using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
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
        public DbSet<VehicleTypeTranslation> VehicleTypeTranslations { get; set; }
        public DbSet<VehicleBrand> VehicleBrands { get; set; }
        public DbSet<VehicleBrandTranslation> VehicleBrandTranslations { get; set; }
        public DbSet<LanguageEntity> Languages { get; set; }
        public DbSet<TourDifficultyEntity> TourDifficulties { get; set; }
        public DbSet<TourDifficultyTranslation> TourDifficultyTranslations { get; set; }
        public DbSet<CustomerUser> CustomerUsers { get; set; }
        public DbSet<TourTypeEnitity> TourTypes { get; set; }
        public DbSet<TourTypeTranslation> TourTypeTranslations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Fluent API konfigürasyonları yapılacaksa buraya
            builder.Entity<VehicleTypeTranslation>().ToTable("VehicleTypeTranslations");
            builder.Entity<VehicleBrandTranslation>().ToTable("VehicleBrandTranslations");
            builder.Entity<TourTypeTranslation>().ToTable("TourTypeTranslations");
            builder.Entity<TourDifficultyTranslation>().ToTable("TourDifficultyTranslations");
        }
    }
}
