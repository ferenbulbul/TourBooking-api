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
        public DbSet<TourPointEntity> TourPoints { get; set; }
        public DbSet<TourPointTranslation> TourPointTranslations { get; set; }
        public DbSet<CountryEntity> Countries { get; set; }
        public DbSet<CountryTranslation> CountryTranslations { get; set; }
        public DbSet<RegionEntity> Regions { get; set; }
        public DbSet<RegionTranslation> RegionTranslations { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<CityTranslation> CityTranslations { get; set; }
        public DbSet<DistrictEntity> Districts { get; set; }
        public DbSet<DistrictTranslation> DistrictTranslations { get; set; }
        public DbSet<DriverEntity> Drivers { get; set; }
        public DbSet<LegroomSpaceEntity> LegroomSpaces { get; set; }
        public DbSet<SeatTypeEntity> SeatTypes { get; set; }
        public DbSet<VehicleClassEntity> VehicleClasses { get; set; }
        public DbSet<VehicleEntity> Vehicles { get; set; }
        public DbSet<TourEntity> Tours { get; set; }
        public DbSet<TourPricingEntity> TourPricings { get; set; }
        public DbSet<GuideUserEntity> Guides { get; set; }
        public DbSet<AgencyUserEntity> Agencies { get; set; }
        public DbSet<AvailabilityEntity> Availabilities => Set<AvailabilityEntity>();
        public DbSet<BusyDayEntity> BusyDays => Set<BusyDayEntity>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Fluent API konfigürasyonları yapılacaksa buraya
            builder.Entity<VehicleTypeTranslation>().ToTable("VehicleTypeTranslations");
            builder.Entity<VehicleBrandTranslation>().ToTable("VehicleBrandTranslations");
            builder.Entity<TourTypeTranslation>().ToTable("TourTypeTranslations");
            builder.Entity<TourDifficultyTranslation>().ToTable("TourDifficultyTranslations");
            builder.Entity<TourPointTranslation>().ToTable("TourPointTranslations");
            builder.Entity<CountryTranslation>().ToTable("CountryTranslations");
            builder.Entity<RegionTranslation>().ToTable("RegionTranslations");
            builder.Entity<CityTranslation>().ToTable("CityTranslations");
            builder.Entity<DistrictTranslation>().ToTable("DistrictTranslations");
            builder.Entity<VehicleClassTranslation>().ToTable("VehicleClassTranslations");
            builder.Entity<SeatTypeTranslation>().ToTable("SeatTypeTranslations");
            builder.Entity<LegroomSpaceTranslation>().ToTable("LegroomSpaceTranslations");
            builder.Entity<AvailabilityEntity>().HasIndex(x => x.VehicleId).IsUnique();

            builder
                .Entity<BusyDayEntity>()
                .HasIndex(x => new { x.AvailabilityId, x.Day })
                .IsUnique(); // aynı günü iki kez yazmayı engelle

            // İlişki
            builder
                .Entity<BusyDayEntity>()
                .HasOne(x => x.Availability)
                .WithMany(a => a.BusyDays)
                .HasForeignKey(x => x.AvailabilityId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<RegionEntity>()
                .HasOne(r => r.Country)
                .WithMany(c => c.Regions)
                .HasForeignKey(r => r.CountryId);

            builder
                .Entity<CityEntity>()
                .HasOne(c => c.Region)
                .WithMany(r => r.Cities)
                .HasForeignKey(c => c.RegionId);

            builder
                .Entity<DistrictEntity>()
                .HasOne(d => d.City)
                .WithMany(c => c.Districts)
                .HasForeignKey(d => d.CityId);
        }
    }
}
