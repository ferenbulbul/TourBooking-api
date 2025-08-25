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
        public DbSet<GuideBlock> GuideBlocks => Set<GuideBlock>();
        public DbSet<AgencyUserEntity> Agencies { get; set; }
        public DbSet<CallCenterAgentEntity> CallCenterAgents { get; set; }
        public DbSet<AvailabilityEntity> Availabilities => Set<AvailabilityEntity>();
        public DbSet<BusyDayEntity> BusyDays => Set<BusyDayEntity>();
        public DbSet<BookingEntity> Bookings => Set<BookingEntity>();
        public DbSet<GuideTourPriceEntity> GuideTourPrices => Set<GuideTourPriceEntity>();
        public DbSet<GuideLanguageEntity> GuideLanguages => Set<GuideLanguageEntity>();
        public DbSet<TourRoutePriceEntity> TourRoutePrices => Set<TourRoutePriceEntity>();
        public DbSet<VehicleBlockEntity> VehicleBlocks => Set<VehicleBlockEntity>();
        public DbSet<DriverLocationEntity> DriverLocationEntities => Set<DriverLocationEntity>();
        public DbSet<CustomerLocationEntity> CustomerLocationEntities => Set<CustomerLocationEntity>();

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

            // Infrastructure/Context/AppDbContext.cs  (OnModelCreating)
            builder.Entity<TourRoutePriceEntity>(b =>
            {
                b.ToTable("TourRoutePrices");
                b.HasKey(x => x.Id);

                // 🔒 Aynı kombinasyon tek olsun (unique)
                b.HasIndex(x => new
                {
                    x.TourPointId,
                    x.CountryId,
                    x.RegionId,
                    x.CityId,
                    x.DistrictId,
                    x.VehicleId,
                    x.DriverId,
                    x.AgencyId
                })
                    .IsUnique();

                b.Property(x => x.Price).HasColumnType("decimal(18,2)");
                b.Property(x => x.Currency).HasMaxLength(8);
            });

            builder.Entity<GuideLanguageEntity>(e =>
            {
                // 1) Composite PK kullanacaksan 'Id'ı kaldır/ignore et
                e.HasKey(x => new { x.GuideId, x.LanguageId });
                e.Ignore(x => x.Id);

                // 2) Guide ilişkisini AÇIKÇA tanımla => shadow FK oluşmasın
                e.HasOne<GuideUserEntity>() // navigation yazmak zorunda değilsin
                    .WithMany(g => g.GuideLanguages)
                    .HasForeignKey(x => x.GuideId)
                    .OnDelete(DeleteBehavior.Cascade);

                // 3) Language ilişkisi
                e.HasOne(gl => gl.Language)
                    .WithMany(l => l.GuideLanguages)
                    .HasForeignKey(gl => gl.LanguageId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Eğer class'ta 'Id' property’si hala duruyorsa, tamamen kaldır
                // ya da:
                // e.Ignore(x => x.Id);
            });

            builder.Entity<GuideTourPriceEntity>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Guide);
                e.Property(x => x.Price).HasColumnType("decimal(10,2)");
                e.Property(x => x.Currency).HasMaxLength(3);
                e.HasIndex(x => new
                {
                    x.GuideId,
                    x.CityId,
                    x.DistrictId,
                    x.TourPointId
                })
                    .IsUnique();
            });

            builder.Entity<BookingEntity>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Guide).WithMany(g => g.Bookings).HasForeignKey(x => x.GuideId);
                e.Property(x => x.Status).HasConversion<int>();
                e.HasIndex(x => new
                {
                    x.GuideId,
                    x.StartDate,
                    x.EndDate
                });
            });

            builder.Entity<GuideBlock>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Guide).WithMany(g => g.Blocks).HasForeignKey(x => x.GuideId);
                e.HasIndex(x => new
                {
                    x.GuideId,
                    x.StartDate,
                    x.EndDate
                });
            });

            builder.Entity<VehicleBlockEntity>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Vehicle).WithMany(g => g.Blocks).HasForeignKey(x => x.VehicleId);
                e.HasIndex(x => new
                {
                    x.VehicleId,
                    x.StartDate,
                    x.EndDate
                });
            });

            builder
                .Entity<GuideUserEntity>()
                .HasOne(c => c.AppUser)
                .WithOne(u => u.GuideUser)
                .HasForeignKey<GuideUserEntity>(c => c.Id);
            builder
                .Entity<CallCenterAgentEntity>()
                .HasOne(c => c.AppUser)
                .WithOne(u => u.CallCenterAgent)
                .HasForeignKey<CallCenterAgentEntity>(c => c.Id);

            builder
                .Entity<AgencyUserEntity>()
                .HasOne(c => c.AppUser)
                .WithOne(u => u.AgencyUser)
                .HasForeignKey<AgencyUserEntity>(c => c.Id);

            builder
                .Entity<CustomerUser>()
                .HasOne(c => c.AppUser)
                .WithOne(u => u.CustomerUser)
                .HasForeignKey<CustomerUser>(c => c.Id);

            builder.Entity<BusyDayEntity>(entity =>
            {
                entity.HasIndex(x => new { x.AvailabilityId, x.Day }).IsUnique();

                entity
                    .HasOne(x => x.Availability)
                    .WithMany(a => a.BusyDays)
                    .HasForeignKey(x => x.AvailabilityId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
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

            builder.Entity<DriverLocationEntity>(e =>
                {
                    e.ToTable("DriverLocation");
                    e.HasKey(x => x.Id);

                    // Driver tablosuna 1–1 (shared PK) bağla (inverse navigation olmadan)
                    e.HasOne<DriverEntity>()       // principal tip
                     .WithOne()              // inverse yok
                     .HasForeignKey<DriverLocationEntity>(x => x.Id)
                     .OnDelete(DeleteBehavior.Cascade);

                    e.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                });
            builder.Entity<CustomerLocationEntity>(e =>
                {
                    e.ToTable("CustomerLocation");
                    e.HasKey(x => x.Id);

                    e.HasOne<CustomerUser>()
                     .WithOne()
                     .HasForeignKey<CustomerLocationEntity>(x => x.Id)
                     .OnDelete(DeleteBehavior.Cascade);

                    e.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                });
            builder
                .Entity<LanguageEntity>()
                .HasData(
                    new LanguageEntity
                    {
                        Id = new Guid("11111111-1111-1111-1111-111111111111"),
                        Name = "Türkçe",
                        Code = "tr",
                        IsActive = true
                    },
                    new LanguageEntity
                    {
                        Id = new Guid("22222222-2222-2222-2222-222222222222"),
                        Name = "English",
                        Code = "en",
                        IsActive = true
                    },
                    new LanguageEntity
                    {
                        Id = new Guid("33333333-3333-3333-3333-333333333333"),
                        Name = "العربية", // Arapça
                        Code = "ar",
                        IsActive = true
                    }
                );
        }
    }
}
