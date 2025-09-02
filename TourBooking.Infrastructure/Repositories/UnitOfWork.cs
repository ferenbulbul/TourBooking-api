using System.Diagnostics;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.Admin;
using TourBooking.Application.DTOs.GuideCalendar;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Features;
using TourBooking.Application.Features.Admin.Query.AgenciesToConfirm;
using TourBooking.Application.Features.Authentication.Queries.IsApproved;
using TourBooking.Application.Features.Mobile.Query.TourPointDetails;
using TourBooking.Application.Features.Settings;
using TourBooking.Application.Features.Settings.Queries;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;
using TourBooking.Infrastructure.Context;

namespace TourBooking.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task RollbackAsync()
        {
            _context.ChangeTracker.Entries().ToList().ForEach(e => e.State = EntityState.Detached);
            return Task.CompletedTask;
        }

        public IGenericRepository<T> GetRepository<T>()
            where T : class, IBaseEntity
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var repoInstance = new GenericRepository<T>(_context);
                _repositories[type] = repoInstance;
            }
            return (IGenericRepository<T>)_repositories[type];
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<TourTypeEnitity>> TourTypes(
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .TourTypes.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .ToListAsync();
        }

        public async Task<IEnumerable<TourDifficultyEntity>> TourDifficulties(
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .TourDifficulties.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleType>> VehicleTypes(
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .VehicleTypes.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .ToListAsync();
        }

        public async Task<VehicleType> VehicleType(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .VehicleTypes.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<TourDifficultyEntity> TourDifficulty(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .TourDifficulties.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<TourTypeEnitity> TourType(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .TourTypes.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<CityEntity>> Cities(
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Cities.Include(a => a.Region)
                .Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .ToListAsync();
        }

        public async Task<IEnumerable<CountryEntity>> Countries(
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Countries.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .ToListAsync();
        }

        public async Task<IEnumerable<RegionEntity>> Regions(
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Regions.Include(a => a.Country)
                .ThenInclude(c => c.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(a => a.Translations)
                .ThenInclude(tt => tt.Language)
                .ToListAsync();
        }

        public async Task<IEnumerable<DistrictEntity>> Districts(
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Districts.Include(t => t.City)
                .Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .ToListAsync();
        }

        public async Task<CityEntity> City(Guid Id, CancellationToken cancellationToken = default)
        {
            return await _context
                .Cities.Include(t => t.Region)
                .Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<CountryEntity> Country(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Countries.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<DistrictEntity> District(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Districts.Include(t => t.City)
                .Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<RegionEntity> Region(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Regions.Include(t => t.Country)
                .Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<TourPointEntity>> TourPoints(
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var res = await _context
                    .TourPoints.Include(t => t.Country)
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(tt => tt.Language)
                    .Include(t => t.Region)
                    .ThenInclude(r => r.Translations)
                    .ThenInclude(tt => tt.Language)
                    .Include(t => t.City)
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(tt => tt.Language)
                    .Include(t => t.District)
                    .ThenInclude(d => d.Translations)
                    .ThenInclude(tt => tt.Language)
                    .Include(t => t.TourDifficulty)
                    .ThenInclude(td => td.Translations)
                    .ThenInclude(tt => tt.Language)
                    .Include(t => t.TourType)
                    .ThenInclude(tt => tt.Translations)
                    .ThenInclude(tt => tt.Language)
                    .Include(t => t.Translations)
                    .ThenInclude(tt => tt.Language)
                    .ToListAsync();

                return res;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<TourPointEntity> TourPoint(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .TourPoints.Include(t => t.Country)
                .Include(t => t.Region)
                .Include(t => t.City)
                .Include(t => t.District)
                .Include(t => t.TourDifficulty)
                .Include(t => t.TourType)
                .Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<DriverEntity> Driver(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context.Drivers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<DriverEntity>> Drivers(
            CancellationToken cancellationToken = default
        )
        {
            return await _context.Drivers.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<VehicleBrand>> VehicleBrands(
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var res = await _context
                    .VehicleBrands.AsNoTracking()
                    .Include(t => t.Translations)
                    .ThenInclude(tt => tt.Language)
                    .ToListAsync();
                return res;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<VehicleBrand> VehicleBrand(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .VehicleBrands.AsNoTracking()
                .Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<SeatTypeEntity> SeatType(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .SeatTypes.AsNoTracking()
                .Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SeatTypeEntity>> SeatTypes(
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .SeatTypes.AsNoTracking()
                .Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .ToListAsync();
        }

        public async Task<IEnumerable<LegroomSpaceEntity>> Legrooms(
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .LegroomSpaces.AsNoTracking()
                .Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .ToListAsync();
        }

        public async Task<LegroomSpaceEntity> Legroom(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .LegroomSpaces.AsNoTracking()
                .Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<VehicleClassEntity>> VehicleClasses(
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .VehicleClasses.AsNoTracking()
                .Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .ToListAsync();
        }

        public async Task<VehicleClassEntity> VehicleClass(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .VehicleClasses.AsNoTracking()
                .Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<VehicleEntity>> Vehicles(
            CancellationToken cancellationToken = default
        )
        {
            return await _context.Vehicles.AsNoTracking().ToListAsync();
        }

        public async Task<VehicleEntity> Vehicle(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context.Vehicles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<TourEntity> Tour(Guid Id, CancellationToken cancellationToken = default)
        {
            return await _context
                .Tours.AsNoTracking()
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.City)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.Country)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.Region)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.District)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.Vehicle)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.Driver)
                .Include(q => q.TourPoint)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<TourEntity>> Tours(
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Tours.AsNoTracking()
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.City)
                .ThenInclude(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.Country)
                .ThenInclude(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.Region)
                .ThenInclude(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.District)
                .ThenInclude(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.Vehicle)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.Driver)
                .Include(t => t.TourPoint)
                .ThenInclude(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .ToListAsync();
        }

        public async Task<AvailabilityEntity> VehicleAvailabilityByVehicleId(
            Guid vehicleId,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Availabilities.AsNoTracking()
                .FirstOrDefaultAsync(x => x.VehicleId == vehicleId);
        }

        public async Task<AvailabilityEntity> VehicleAvailabilityBy(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Availabilities.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<AvailabilityEntity>> VehicleAvailabilities(
            CancellationToken cancellationToken = default
        )
        {
            return await _context.Availabilities.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<MobileHighlightedTourPointDto>> HighlightedTourPoints(
            CancellationToken cancellationToken = default
        )
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var dtos2 =
            await _context.TourPoints.AsNoTracking().Where(t => t.IsHighlighted)
            .Select(x => new MobileHighlightedTourPointDto
            {
                Id = x.Id,
                CityId = x.CityId,
                CityName = x.City
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault() ?? "",
                CountryId = x.CountryId,
                CountryName = x.Country
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault() ?? "",
                RegionId = x.RegionId,
                RegionName = x.Region
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault() ?? "",
                TourTypeId = x.TourTypeId,
                TourTypeName = x.TourType
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault() ?? "",
                MainImage = x.MainImage,
                Title = x.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault() ?? "",
                Description = x.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Description)
                        .FirstOrDefault() ?? "",
            }).ToListAsync();
            return dtos2;
        }

        public async Task<IEnumerable<DriverEntity>> DriversForAgency(
            Guid agencyId,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Drivers.AsNoTracking()
                .Where(d => d.AgencyId == agencyId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<VehicleEntity>> VehiclesForAgency(
            Guid agencyId,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                return await _context
                    .Vehicles.AsNoTracking()
                    .Where(v => v.AgencyId == agencyId)
                    .ToListAsync(cancellationToken);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TourEntity>> ToursForAgency(
            Guid agencyId,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Tours.AsNoTracking()
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.City)
                .ThenInclude(tt => tt.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.Country)
                .ThenInclude(tt => tt.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.Region)
                .ThenInclude(tt => tt.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.District)
                .ThenInclude(tt => tt.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.Vehicle)
                .Include(t => t.PricingEntity)
                .ThenInclude(pe => pe.Driver)
                .Include(t => t.TourPoint)
                .ThenInclude(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .Where(t => t.AgencyId == agencyId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MobileTourPointsBySearchDto>> MobileTourPointBySearch(
            string searchQuery,
            string culture,
            CancellationToken cancellationToken = default
        )
        {
            searchQuery = searchQuery.ToLower();

            var tours = await _context
                .TourPoints.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .Where(t =>
                    t.Translations.Any(tr =>
                        tr.Language.Code == culture && tr.Title.ToLower().StartsWith(searchQuery)
                    )
                )
                .Select(t => new MobileTourPointsBySearchDto
                {
                    Name = t
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),
                    Type = "Tour",
                    Id = t.Id
                })
                .ToListAsync();

            var cities = await _context
                .Cities.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .Where(t =>
                    t.Translations.Any(tr =>
                        tr.Language.Code == culture && tr.Title.ToLower().StartsWith(searchQuery)
                    )
                )
                .Select(t => new MobileTourPointsBySearchDto
                {
                    Name = t
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),
                    Type = "City",
                    Id = t.Id
                })
                .ToListAsync();

            var combinedResults = tours.Concat(cities).ToList();

            return combinedResults;
        }

        public async Task<IEnumerable<MobileCityDto>> CitiesForMobile(
            Guid regionId,
            string culture,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Cities.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .Where(t =>
                    t.RegionId == regionId && t.Translations.Any(tr => tr.Language.Code == culture)
                )
                .Select(t => new MobileCityDto
                {
                    Name = t
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),
                    Id = t.Id
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MobileRegionDto>> RegionsForMobile(
            string culture,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Regions.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .Where(t => t.Translations.Any(tr => tr.Language.Code == culture))
                .Select(t => new MobileRegionDto
                {
                    Name = t
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),
                    Id = t.Id
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MobileDistrictDto>> DistrictsForMobile(
            Guid cityId,
            string culture
        )
        {
            return await _context
                .Districts.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .Where(t =>
                    t.CityId == cityId && t.Translations.Any(tr => tr.Language.Code == culture)
                )
                .Select(t => new MobileDistrictDto
                {
                    Name = t
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),
                    Id = t.Id
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MobileDetailedSearchResultDto>> MobileTourPointsByLocation(
            Guid? regionId,
            Guid? cityId,
            Guid? districtId,
            string culture
        )
        {
            var query = _context
                .TourPoints.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .Where(tp => tp.RegionId == regionId);

            if (cityId.HasValue)
            {
                query = query.Where(tp => tp.CityId == cityId.Value);
            }

            if (districtId.HasValue)
            {
                query = query.Where(tp => tp.DistrictId == districtId.Value);
            }

            return await query
                .Select(t => new MobileDetailedSearchResultDto
                {
                    Name = t
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),
                    Id = t.Id,
                    TourTypeId = t.TourTypeId,
                    TourTypeName = t
                        .TourType.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),
                    TourDifficultyId = t.TourDifficultyId,
                    TourDifficultyName = t
                        .TourDifficulty.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),
                    CountryId = t.CountryId,
                    CountryName = t
                        .Country.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),
                    RegionId = t.RegionId,
                    RegionName = t
                        .Region.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),
                    CityId = t.CityId,
                    CityName = t
                        .City.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),
                    DistrictId = t.DistrictId,
                    DistrictName = t
                        .District.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),
                    MainImage = t.MainImage,
                    OtherImages = t.OtherImages
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MobileDetailedSearchResultDto>> MobileTourPointsByDeparture(
            Guid? regionId,
            Guid? cityId,
            Guid? districtId,
            string culture
        )
        {
            // 1) Uygun route’lar: kalkış (region/city/district) filtreleri
            var baseRoutes = _context
                .TourRoutePrices.AsNoTracking()
                .Where(rp =>
                    (!regionId.HasValue || rp.RegionId == regionId.Value)
                    && (!cityId.HasValue || rp.CityId == cityId.Value)
                    && (!districtId.HasValue || rp.DistrictId == districtId.Value)
                );

            // 2) Tekil TourPointId seti
            var tourPointIds = baseRoutes.Select(rp => rp.TourPointId).Distinct();

            // 3) TourPoint detaylarını projekte et (dil çevirileri dahil)
            var list = await _context
                .TourPoints.AsNoTracking()
                .Where(tp => tourPointIds.Contains(tp.Id))
                .Select(tp => new MobileDetailedSearchResultDto
                {
                    Id = tp.Id,
                    Name = tp
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    TourTypeId = tp.TourTypeId,
                    TourTypeName = tp
                        .TourType.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    TourDifficultyId = tp.TourDifficultyId,
                    TourDifficultyName = tp
                        .TourDifficulty.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    CountryId = tp.CountryId,
                    CountryName = tp
                        .Country.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    RegionId = tp.RegionId,
                    RegionName = tp
                        .Region.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    CityId = tp.CityId,
                    CityName = tp
                        .City.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    DistrictId = tp.DistrictId,
                    DistrictName = tp
                        .District.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    MainImage = tp.MainImage,
                    OtherImages = tp.OtherImages
                })
                .ToListAsync();
            return list;
        }

        public async Task<IEnumerable<MobileDetailedSearchResultDto>> MobileTourPointByTourTypeId(
            Guid? tourType,
            string culture
        )
        {
            // 1) Uygun route’lar: kalkış (region/city/district) filtreleri
            var baseRoutes = _context
                .TourRoutePrices.AsNoTracking()
                .Where(rp => rp.TourPoint.TourType.Id == tourType);

            // 2) Tekil TourPointId seti
            var tourPointIds = baseRoutes.Select(rp => rp.TourPointId).Distinct();

            // 3) TourPoint detaylarını projekte et (dil çevirileri dahil)
            var list = await _context
                .TourPoints.AsNoTracking()
                .Where(tp => tourPointIds.Contains(tp.Id))
                .Select(tp => new MobileDetailedSearchResultDto
                {
                    Id = tp.Id,
                    Name = tp
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    TourTypeId = tp.TourTypeId,
                    TourTypeName = tp
                        .TourType.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    TourDifficultyId = tp.TourDifficultyId,
                    TourDifficultyName = tp
                        .TourDifficulty.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    CountryId = tp.CountryId,
                    CountryName = tp
                        .Country.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    RegionId = tp.RegionId,
                    RegionName = tp
                        .Region.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    CityId = tp.CityId,
                    CityName = tp
                        .City.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    DistrictId = tp.DistrictId,
                    DistrictName = tp
                        .District.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    MainImage = tp.MainImage,
                    OtherImages = new List<string>(),
                })
                .ToListAsync();
            return list;
        }

        public async Task<MobileTourPointDetailDto> MobileTourPointDetail(Guid tourPointId, string culture, Guid userId
        )
        {
            Stopwatch bestWatch = new();
            bestWatch.Start();
            var t = await _context
                .TourPoints.Where(tp => tp.Id == tourPointId)
                .Select(tp => new
                {
                    IsFavorite = tp.Favorites.Any(f => f.CustomerId == userId),
                    tp.Id,
                    tp.MainImage,
                    tp.OtherImages,
                    Title = tp
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),
                    Description = tp
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Description)
                        .FirstOrDefault(),

                    CountryName = tp
                        .Country.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    RegionName = tp
                        .Region.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    CityName = tp
                        .City.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    DistrictName = tp
                        .District.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    TourTypeName = tp
                        .TourType.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    TourDifficultyName = tp
                        .TourDifficulty.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    // Şimdi Cities için sadece Id alıyoruz
                    PricingCities = tp
                        .RoutePrices.Where(pe => pe.City != null)
                        .Select(pe => new
                        {
                            CityId = pe.City.Id,
                            CityName = pe
                                .City.Translations.Where(tr => tr.Language.Code == culture)
                                .Select(tr => tr.Title)
                                .FirstOrDefault()
                        }),

                    PricingDistricts = tp
                        .RoutePrices.Where(pe => pe.District != null)
                        .Select(pe => new
                        {
                            pe.DistrictId,
                            pe.CityId,
                            DistrictName = pe
                                .District.Translations.Where(tr => tr.Language.Code == culture)
                                .Select(tr => tr.Title)
                                .Take(1)
                                .FirstOrDefault()
                        })
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            // TODO : Performans bakılacak.

            // Ardından bellek tarafında `DistinctBy` uygula
            var result = new MobileTourPointDetailDto
            {
                Id = t.Id,
                Title = t.Title ?? string.Empty,
                CountryName = t.CountryName ?? string.Empty,
                RegionName = t.RegionName ?? string.Empty,
                CityName = t.CityName ?? string.Empty,
                DistrictName = t.DistrictName ?? string.Empty,
                TourTypeName = t.TourTypeName ?? string.Empty,
                TourDifficultyName = t.TourDifficultyName ?? string.Empty,
                MainImage = t.MainImage,
                OtherImages = t.OtherImages,
                Description = t.Description,
                IsFavorites = t.IsFavorite,
                Cities = t
                    .PricingCities.GroupBy(c => c.CityId)
                    .Select(g => new MobileCityDto
                    {
                        Id = g.Key,
                        Name = g.First().CityName ?? string.Empty
                    })
                    .ToList(),

                Districts = t
                    .PricingDistricts.GroupBy(d => new { d.CityId, d.DistrictId }) // Id burada DistrictId oluyor
                    .Select(g => new MobileDistrictDto2
                    {
                        Id = g.Key.DistrictId, // DistrictId
                        CityId = g.Key.CityId,
                        Name = g.First().DistrictName ?? string.Empty
                    })
                    .ToList()
            };
            bestWatch.Stop();
            Console.WriteLine(bestWatch.ElapsedMilliseconds);
            Console.WriteLine("fazlı" + userId + t.IsFavorite + result.IsFavorites);
            Console.WriteLine("emo" + JsonConvert.SerializeObject(t));

            return result;
        }

        public async Task<IEnumerable<MobileSearchVehicleDto>> MobileSearchVehicles(
            MobileSearchVehiclesQuery request
        )
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var result = await _context
                .TourRoutePrices.AsNoTracking()
                .Where(tp =>
                    tp.CityId == request.CityId
                    && tp.DistrictId == request.DistrictId
                    && tp.TourPointId == request.TourPointId
                    && !tp.Vehicle.Blocks.Any(a => a.StartDate == request.Date)
                )
                .Select(tp => new MobileSearchVehicleDto
                {
                    VehicleId = tp.VehicleId,
                    Price = tp.Price,
                    VehicleBrand =
                        tp.Vehicle.VehicleBrand.Translations.Where(x => x.Language.Code == culture)
                            .FirstOrDefault()
                            .Title ?? string.Empty,
                    VehicleClass =
                        tp.Vehicle.VehicleClass.Translations.Where(x => x.Language.Code == culture)
                            .FirstOrDefault()
                            .Title ?? string.Empty,
                    VehicleType =
                        tp.Vehicle.VehicleType.Translations.Where(x => x.Language.Code == culture)
                            .FirstOrDefault()
                            .Title ?? string.Empty,
                    SeatCount = tp.Vehicle.SeatCount,
                    Image = tp.Vehicle.AracResmi,
                })
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<MobileSearchGuidesDto>> MobileSearchGuides(
            MobileSearchGuideQuery request
        )
        {
            var culture = CultureInfo.CurrentUICulture.Name;

            var result = await _context
                .GuideTourPrices.AsNoTracking()
                .Where(tp =>
                    tp.CityId == request.CityId
                    && tp.DistrictId == request.DistrictId
                    && tp.TourPointId == request.TourPointId
                    && !tp.Guide.Blocks.Any(a =>
                        a.StartDate == request.Date && a.EndDate == request.Date
                    )
                )
                .Select(tp => new MobileSearchGuidesDto
                {
                    GuideId = tp.GuideId,
                    Price = tp.Price,
                    FirstName = tp.Guide.FirstName,
                    LastName = tp.Guide.LastName,
                    Image = tp.Guide.PhotoUrl,
                    Languages = tp.Guide.GuideLanguages.Select(c => c.Language.Name).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        public async Task<MobileDetailVehicleDto> MobileDetailVehicle(Guid vehicleId)
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var result = await _context
                .Vehicles.AsNoTracking()
                .Where(tp => tp.Id == vehicleId)
                .Select(tp => new MobileDetailVehicleDto
                {
                    VehicleBrand =
                        tp.VehicleBrand.Translations.Where(x => x.Language.Code == culture)
                            .FirstOrDefault()
                            .Title ?? string.Empty,
                    VehicleClass =
                        tp.VehicleClass.Translations.Where(x => x.Language.Code == culture)
                            .FirstOrDefault()
                            .Title ?? string.Empty,
                    VehicleType =
                        tp.VehicleType.Translations.Where(x => x.Language.Code == culture)
                            .FirstOrDefault()
                            .Title ?? string.Empty,
                    SeatCount = tp.SeatCount,
                    ModelYear = tp.ModelYear,
                    SeatType =
                        tp.SeatType.Translations.Where(x => x.Language.Code == culture)
                            .FirstOrDefault()
                            .Title ?? string.Empty,
                    LegRoomSpace =
                        tp.LegRoomSpace.Translations.Where(x => x.Language.Code == culture)
                            .FirstOrDefault()
                            .Title ?? string.Empty,
                    OtherImages = tp.OtherImages,
                    VehicleFeatures = tp.VehicleFeatures,
                    Image = tp.AracResmi,
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<MobileTourBookingSummaryVehicleTourDto> TourBookingVehicleTourSummary(
            Guid tourPointId,
            Guid districtId,
            Guid vehicleId,
            DateOnly? date
        )
        {
            var culture = CultureInfo.CurrentUICulture.Name; // Tercihen parametre olarak gelmeli

            var result = await _context
                .TourRoutePrices.AsNoTracking()
                .Where(tp =>
                    tp.TourPointId == tourPointId
                    && tp.VehicleId == vehicleId
                    && tp.DistrictId == districtId
                    && !tp.Vehicle.Blocks.Any(a => a.StartDate == date)
                )
                .Select(tp => new MobileTourBookingSummaryVehicleTourDto
                {
                    TourPointTitle =
                        tp.TourPoint.Translations.Where(x => x.Language.Code == culture)
                            .Select(x => x.Title)
                            .FirstOrDefault() ?? string.Empty,

                    TourPointCity =
                        tp.TourPoint.City.Translations.Where(x => x.Language.Code == culture)
                            .Select(x => x.Title)
                            .FirstOrDefault() ?? string.Empty,

                    TourPointDistrict =
                        tp.TourPoint.District.Translations.Where(x => x.Language.Code == culture)
                            .Select(x => x.Title)
                            .FirstOrDefault() ?? string.Empty,

                    CarBrand =
                        tp.Vehicle.VehicleBrand.Translations.Where(x => x.Language.Code == culture)
                            .Select(x => x.Title)
                            .FirstOrDefault() ?? string.Empty,

                    DriverName = tp.Driver.NameSurname ?? string.Empty,
                    TourPrice = tp.Price
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<MobileTourBookingSummaryGuideDto> TourBookingGuideSummary(
            Guid guideId,
            Guid districtId,
            Guid tourPointId,
            DateOnly? date
        )
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var result = await _context
                .GuideTourPrices.AsNoTracking()
                .Where(tp =>
                    tp.GuideId == guideId
                    && tp.TourPointId == tourPointId
                    && tp.DistrictId == districtId
                    && tp.Guide.Blocks.Any(a => a.StartDate == date && a.EndDate == date)
                )
                .Select(tp => new MobileTourBookingSummaryGuideDto
                {
                    GuidName = tp.Guide.FirstName,
                    GuidePrice = tp.Price
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<TourRoutePriceEntity> ControlTourRoute(
            Guid tourPointId,
            Guid cityId,
            Guid districtId,
            Guid vehicleId,
            decimal price
        )
        {
            return await _context
                .TourRoutePrices.AsNoTracking()
                .Where(tp =>
                    tp.TourPointId == tourPointId
                    && tp.VehicleId == vehicleId
                    && tp.CityId == cityId
                    && tp.DistrictId == districtId
                    && tp.Price == price
                )
                .FirstOrDefaultAsync();
        }

        public async Task<VehicleBlockEntity> ControlVehicleAvalibity(Guid vehicleId, DateOnly date)
        {
            return await _context
                .VehicleBlocks.AsNoTracking()
                .Where(tp =>
                    tp.VehicleId == vehicleId && tp.StartDate == date && tp.EndDate == date
                )
                .FirstOrDefaultAsync();
        }

        public async Task<GuideTourPriceEntity> ControlGuideAvalibity(
            Guid guideId,
            decimal price,
            DateOnly date,
            Guid tourPointId,
            Guid districtId,
            Guid cityId
        )
        {
            return await _context
                .GuideTourPrices.AsNoTracking()
                .Where(tp =>
                    tp.GuideId == guideId
                    && tp.TourPointId == tourPointId
                    && tp.CityId == cityId
                    && tp.DistrictId == districtId
                    && tp.Price == price
                    && !tp.Guide.Blocks.Any(a => a.StartDate == date && a.EndDate == date)
                )
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CalendarEventDto2>> GuideEvents(FetchEventsQuery request)
        {
            // Bookings (read-only)
            var bookings = await _context
                .Bookings.Where(x =>
                    x.GuideId == request.GuidId
                    && x.StartDate <= request.To
                    && request.From <= x.EndDate
                    && x.Status == BookingStatus.Confirmed
                )
                .Select(x => new CalendarEventDto2(
                    null,
                    "Rezerve",
                    x.StartDate,
                    x.EndDate,
                    false,
                    "#dc2626" // kırmızı
                ))
                .ToListAsync();

            // Guide Blocks (editable)
            var blocks = await _context
                .GuideBlocks.Where(x =>
                    x.GuideId == request.GuidId
                    && x.StartDate <= request.To
                    && request.From <= x.EndDate
                )
                .Select(x => new CalendarEventDto2(
                    x.Id,
                    "Meşgul",
                    x.StartDate,
                    x.EndDate,
                    true,
                    "#6b7280" // gri
                ))
                .ToListAsync();

            return bookings.Concat(blocks).ToList();
        }

        public async Task<IEnumerable<CalendarEventDto2>> VehicleEvents(
            FetchVehicleEventsQuery request
        )
        {
            // Bookings (read-only)
            var bookings = await _context.Bookings
    .Where(x =>
        x.VehicleId == request.VehicleId &&
        x.StartDate <= request.To &&
        request.From <= x.EndDate &&
        x.Status == BookingStatus.Confirmed)
    .Select(x => new CalendarEventDto2(
        null,
        "Rezerve",
        x.StartDate,
        x.EndDate,
        false,
        "#dc2626" // kırmızı
    ))
    .ToListAsync();

            // Blocks: confirmed booking ile ÖRTÜŞENLERİ ELEMEl
            var blocks = await _context.VehicleBlocks
                .Where(x =>
                    x.VehicleId == request.VehicleId &&
                    x.StartDate <= request.To &&
                    request.From <= x.EndDate &&
                    !_context.Bookings.Any(b =>
                        b.VehicleId == x.VehicleId &&
                        b.Status == BookingStatus.Confirmed &&
                        // interval overlap: [b.Start,b.End] ∩ [x.Start,x.End] ≠ ∅
                        b.StartDate <= x.EndDate &&
                        x.StartDate <= b.EndDate
                    )
                )
                 .Select(x => new CalendarEventDto2(
                                x.Id,
                            "Meşgul",
                            x.StartDate,
                            x.EndDate,
                            true,
                            "#6b7280" // gri
                        ))
                        .ToListAsync();

            return bookings.Concat(blocks).ToList();
        }

        public async Task CreateGuideBlock(CreateBlockCommand request)
        {
            bool overlapsBooking = await _context.Bookings.AnyAsync(b =>
                b.GuideId == request.GuideId
                && b.Status != BookingStatus.Pending
                && b.StartDate <= request.End
                && request.Start <= b.EndDate
            );
            if (overlapsBooking)
                throw new InvalidOperationException("Seçilen aralıkta müşteri rezervasyonu var.");

            bool overlapsBlock = await _context.GuideBlocks.AnyAsync(gb =>
                gb.GuideId == request.GuideId
                && gb.StartDate <= request.End
                && request.Start <= gb.EndDate
            );
            if (overlapsBlock)
                throw new InvalidOperationException("Seçilen aralık mevcut bir blokla çakışıyor.");

            _context.GuideBlocks.Add(
                new GuideBlock
                {
                    Id = Guid.NewGuid(),
                    GuideId = request.GuideId,
                    StartDate = request.Start,
                    EndDate = request.End,
                    Note = request.Note,
                    CreatedDate = DateTime.UtcNow
                }
            );
            await _context.SaveChangesAsync();
        }

        public async Task CreateVehicleBlock(CreateVehicleBlockCommand request)
        {
            bool overlapsBooking = await _context.Bookings.AnyAsync(b =>
                b.VehicleId == request.VehicleId
                && b.Status == BookingStatus.Confirmed
                && b.StartDate <= request.End
                && request.Start <= b.EndDate
            );
            if (overlapsBooking)
                throw new InvalidOperationException("Seçilen aralıkta müşteri rezervasyonu var.");

            bool overlapsBlock = await _context.VehicleBlocks.AnyAsync(gb =>
                gb.VehicleId == request.VehicleId
                && gb.StartDate <= request.End
                && request.Start <= gb.EndDate
            );
            if (overlapsBlock)
                throw new InvalidOperationException("Seçilen aralık mevcut bir blokla çakışıyor.");

            _context.VehicleBlocks.Add(
                new VehicleBlockEntity
                {
                    Id = Guid.NewGuid(),
                    VehicleId = request.VehicleId,
                    StartDate = request.Start,
                    EndDate = request.End,
                    Note = request.Note,
                    CreatedDate = DateTime.UtcNow
                }
            );
            await _context.SaveChangesAsync();
        }

        public async Task RemoveGuideBlock(RemoveBlockCommand request)
        {
            var block = await _context.GuideBlocks.FirstOrDefaultAsync(x =>
                x.Id == request.BlockId && x.GuideId == request.GuideId
            );
            if (block is null)
                throw new KeyNotFoundException();
            _context.GuideBlocks.Remove(block);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveVehicleBlock(RemoveVehicleBlockCommand request)
        {
            var block = await _context.VehicleBlocks.FirstOrDefaultAsync(x =>
                x.Id == request.BlockId && x.VehicleId == request.VehicleId
            );
            if (block is null)
                throw new KeyNotFoundException();
            _context.VehicleBlocks.Remove(block);
            await _context.SaveChangesAsync();
        }

        public async Task<Guid> FinishBooking(CreateBookingCommand request, Guid driverId, Guid agencyId)
        {
            var entity = new BookingEntity();

            entity.AgencyId = agencyId;
            entity.DriverId = driverId;
            entity.CreatedDate = DateTime.Now;
            entity.CreatedAt = DateTime.Now;
            entity.CustomerId = request.CustomerId ?? throw new InvalidOperationException("CustomerId boş");
            entity.VehicleId = request.VehicleId;
            entity.EndDate = request.Date;
            entity.StartDate = request.Date;
            entity.FromCityId = request.CityId;
            entity.FromDistrictId = request.DistrictId;
            entity.GuideId = request.GuideId;
            entity.TourPointId = request.TourPointId;
            entity.TotalPrice = request.TourPrice + (request.GuidePrice ?? 0);
            entity.LocationDescription = request.LocationDescription;
            entity.Latitude = request.Latitude;
            entity.Longitude = request.Longitude;
            entity.DepartureTime = request.DepartureTime;
            await _context.Bookings.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        // public async Task CreateVehicleBlock(Guid vehicleId, DateOnly date)
        // {
        //     var availability = await _context
        //         .VehicleBlocks
        //         .FirstOrDefaultAsync(a => a.VehicleId == vehicleId);

        //     if (availability == null)
        //     {
        //         availability = new VehicleBlockEntity
        //         {
        //             StartDate=date,
        //             EndDate=date,
        //             VehicleId = vehicleId,
        //         };
        //         _context.VehicleBlocks.Add(availability);
        //     }

        //     await _context.SaveChangesAsync();
        // }

        public async Task<bool> IsUserApproved(IsApprovedQuery request)
        {
            if (request.Role == "Guide")
            {
                var a = await _context
                    .Guides.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.UserId);
                if (a is null)
                    throw new KeyNotFoundException();
                return a.IsConfirmed;
            }
            else if (request.Role == "Agency")
            {
                var a = await _context
                    .Agencies.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.UserId);
                if (a is null)
                    throw new KeyNotFoundException();
                return a.IsConfirmed;
            }

            return true;
        }

        public async Task<IEnumerable<AgencyToConfirmDto>> GetAgenciesToConfirm()
        {
            return await _context
                .Agencies.AsNoTracking()
                .Where(x => x.IsConfirmed == false)
                .Select(tt => new AgencyToConfirmDto(
                    tt.Id,
                    tt.AuthorizedUserFirstName,
                    tt.AuthorizedUserLastName,
                    tt.CompanyName,
                    tt.City,
                    tt.FullAddress,
                    tt.Email,
                    tt.PhoneNumber,
                    tt.PhoneNumber2,
                    tt.TursabUrl,
                    tt.TaxNumber
                ))
                .ToListAsync();
        }

        public async Task<IEnumerable<GuideToConfirmDto>> GetGuidesToConfirm()
        {
            return await _context
                .Guides.AsNoTracking()
                .Where(x => x.IsConfirmed == false)
                .Select(tt => new GuideToConfirmDto(
                    tt.Id,
                    tt.FirstName,
                    tt.LastName,
                    tt.Email,
                    tt.PhoneNumber,
                    tt.DomesticUrl,
                    tt.RegionalUrl
                ))
                .ToListAsync();
        }

        public async Task ConfirmGuide(Guid ıd)
        {
            var guide = await _context.Guides.FirstOrDefaultAsync(x => x.Id == ıd); // tracking açık
            if (guide != null)
            {
                guide.IsConfirmed = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ConfirmAgency(Guid ıd)
        {
            var agency = await _context.Agencies.FirstOrDefaultAsync(x => x.Id == ıd);
            if (agency != null)
            {
                agency.IsConfirmed = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CallCenterAgentEntity> CallCenterAgent(
            Guid Id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context.CallCenterAgents.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<List<DriverLocationDto>> DriverLocations()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul"); // Linux için
                                                                             // Windows'ta "Turkey Standard Time" kullan
            var nowTr = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
            var today = DateOnly.FromDateTime(nowTr);

            var list = await _context.Bookings
                .AsNoTracking()
                .Where(x => x.StartDate == today && x.Status == BookingStatus.Confirmed)
                .Select(yy => new DriverLocationDto(
                    yy.DriverId,
                    yy.Driver.NameSurname,
                    yy.Vehicle.LicensePlate,
                    yy.Driver.DriverLocation.Latitude,
                    yy.Driver.DriverLocation.Longitude,
                    yy.Agency.CompanyName
                ))
                .ToListAsync();
            return list;
        }

        public async Task<SystemCountDto> SystemCounts()
        {
            var dto = new SystemCountDto(
            AgencyCount: await _context.Agencies.AsNoTracking().Where(x => x.IsConfirmed).CountAsync(),
            GuideCount: await _context.Guides.AsNoTracking().Where(x => x.IsConfirmed).CountAsync(),
            VehicleCount: await _context.Vehicles.AsNoTracking().Where(x => x.IsActive).CountAsync(),
            TourPointCount: await _context.TourPoints.AsNoTracking().Where(x => x.IsActive).CountAsync(),
            BookingCount: await _context.Bookings.AsNoTracking().Where(x => x.Status == BookingStatus.Confirmed).CountAsync(),
            CustomerCount: await _context.CustomerUsers.AsNoTracking().Where(x => !x.IsDeleted).CountAsync(),
            TourRouteCount: await _context.TourRoutePrices.AsNoTracking().Where(x => !x.IsDeleted).CountAsync(),
            GuideRouteCount: await _context.GuideTourPrices.AsNoTracking().CountAsync()
            );

            return dto;
        }

        public async Task<FavoriteEntity> ToggleFavoriteAsync(Guid customerId, Guid tourPointId)
        {
            return await _context.Favorites.FirstOrDefaultAsync(x => x.CustomerId == customerId && x.TourPointId == tourPointId);
        }

        public async Task<IEnumerable<MobileHighlightedTourPointDto>> CustomerFavorites(Guid customerId,
            CancellationToken cancellationToken = default
        )
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var dtos2 =
            await _context.Favorites.AsNoTracking().Where(t => t.CustomerId == customerId)
            .Select(x => new MobileHighlightedTourPointDto
            {
                Id = x.Id,
                CityId = default,
                CityName = x.TourPoint.City
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault() ?? "",
                CountryId = default,
                CountryName = string.Empty,
                RegionId = default,
                RegionName = "",
                TourTypeId = default,
                TourTypeName = "",
                MainImage = x.TourPoint.MainImage,
                Title = x.TourPoint.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault() ?? "",
                Description = "",
            }).ToListAsync();
            return dtos2;
        }

        public async Task<IEnumerable<NearbyTourPointDto>> NearbyTourPoints(Guid customerId, CancellationToken cancellationToken = default)
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var customerLocation = await _context.CustomerLocationEntities.Where(x => x.Id == customerId).FirstOrDefaultAsync();
            if (customerLocation != null)
            {


                var nearestTourPoints = _context.TourPoints
        .AsNoTracking()
        .AsEnumerable() // buradan sonrası memory'de çalışır
    .Select(tp => new NearbyTourPointDto
    (
        tp.Id,
        tp.City
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault() ?? "",
        tp.TourType
                        .Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault() ?? "",

        tp.Translations.Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault() ?? "",
        tp.MainImage,
        CalculateDistance(customerLocation.Latitude, customerLocation.Longitude, tp.Lat, tp.Long)
    ))
    .OrderBy(x => x.Distance)
    .Take(5)
    .ToList();
                return nearestTourPoints;
            }
            return new List<NearbyTourPointDto>();
        }
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // km
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            Console.WriteLine(lat1 + lat2+lon1+lon2);
            return R * c;
        }

    }
}
