using System.Diagnostics;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TourBooking.Application.DTOs.Admin;
using TourBooking.Application.DTOs.GuideCalendar;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Features;
using TourBooking.Application.Features.Mobile.Query.TourPointDetails;
using TourBooking.Application.Features.Admin.Query.AgenciesToConfirm;
using TourBooking.Application.Features.Authentication.Queries.IsApproved;
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

        public async Task<IEnumerable<TourPointEntity>> HighlightedTourPoints(
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .TourPoints.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.Country)
                .ThenInclude(tt => tt.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.Region)
                .ThenInclude(tt => tt.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.City)
                .ThenInclude(tt => tt.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.TourType)
                .ThenInclude(tt => tt.Translations)
                .ThenInclude(tt => tt.Language)
                .Where(t => t.IsHighlighted)
                .ToListAsync();
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

        public async Task<IEnumerable<TourPointEntity>> MobileTourPointByTourTypeId(
            Guid TourTypeId,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .TourPoints.Include(t => t.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.Country)
                .ThenInclude(tt => tt.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.Region)
                .ThenInclude(tt => tt.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.City)
                .ThenInclude(tt => tt.Translations)
                .ThenInclude(tt => tt.Language)
                .Include(t => t.TourType)
                .ThenInclude(tt => tt.Translations)
                .ThenInclude(tt => tt.Language)
                .Where(t => t.TourTypeId == TourTypeId)
                .ToListAsync();
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
                Guid? regionId, Guid? cityId, Guid? districtId, string culture)
        {
            // 1) Uygun route’lar: kalkış (region/city/district) filtreleri
            var baseRoutes = _context.TourRoutePrices
               .AsNoTracking()
               .Where(rp => (!regionId.HasValue || rp.RegionId == regionId.Value)
              && (!cityId.HasValue || rp.CityId == cityId.Value)
              && (!districtId.HasValue || rp.DistrictId == districtId.Value));


            // 2) Tekil TourPointId seti
            var tourPointIds = baseRoutes
                .Select(rp => rp.TourPointId)
                .Distinct();

            // 3) TourPoint detaylarını projekte et (dil çevirileri dahil)
            var list = await _context.TourPoints
                .AsNoTracking()
                .Where(tp => tourPointIds.Contains(tp.Id))
                .Select(tp => new MobileDetailedSearchResultDto
                {
                    Id = tp.Id,
                    Name = tp.Translations
                        .Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    TourTypeId = tp.TourTypeId,
                    TourTypeName = tp.TourType.Translations
                        .Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    TourDifficultyId = tp.TourDifficultyId,
                    TourDifficultyName = tp.TourDifficulty.Translations
                        .Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    CountryId = tp.CountryId,
                    CountryName = tp.Country.Translations
                        .Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    RegionId = tp.RegionId,
                    RegionName = tp.Region.Translations
                        .Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    CityId = tp.CityId,
                    CityName = tp.City.Translations
                        .Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    DistrictId = tp.DistrictId,
                    DistrictName = tp.District.Translations
                        .Where(tr => tr.Language.Code == culture)
                        .Select(tr => tr.Title)
                        .FirstOrDefault(),

                    MainImage = tp.MainImage,
                    OtherImages = tp.OtherImages
                })
                .ToListAsync();
                    MainImage = tp.MainImage,
                    OtherImages = tp.OtherImages
                })
                .ToListAsync();

            return list;
        }


        public async Task<MobileTourPointDetailDto> MobileTourPointDetail(
            Guid tourPointId,
            string culture
        )
        {
            Stopwatch bestWatch = new();
            bestWatch.Start();
            var t = await _context
                .TourPoints.Where(tp => tp.Id == tourPointId)
                .Select(tp => new
                {
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
                        })
,

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
                    && !tp.Vehicle.AvailabilityList.Any(a =>
                        a.BusyDays.Any(bd => bd.Day == request.Date)
                    )
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

            var result = await _context.GuideTourPrices.AsNoTracking()
                   .Where(tp =>
                       tp.CityId == request.CityId
                       && tp.DistrictId == request.DistrictId
                       && tp.TourPointId == request.TourPointId
                       && !tp.Guide.Blocks.Any(a =>
                           a.StartDate == request.Date && a.EndDate == request.Date
                       ))
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
    Guid tourPointId, Guid districtId, Guid vehicleId, DateOnly? date)
        {
            var culture = CultureInfo.CurrentUICulture.Name; // Tercihen parametre olarak gelmeli

            var result = await _context.TourRoutePrices
                .AsNoTracking()
                .Where(tp => tp.TourPointId == tourPointId
                          && tp.VehicleId == vehicleId
                          && tp.DistrictId == districtId
                          && !tp.Vehicle.AvailabilityList.Any(a =>
                        a.BusyDays.Any(bd => bd.Day == date)
                          ))
                .Select(tp => new MobileTourBookingSummaryVehicleTourDto
                {
                    TourPointTitle = tp.TourPoint.Translations
                        .Where(x => x.Language.Code == culture)
                        .Select(x => x.Title)
                        .FirstOrDefault() ?? string.Empty,

                    TourPointCity = tp.TourPoint.City.Translations
                        .Where(x => x.Language.Code == culture)
                        .Select(x => x.Title)
                        .FirstOrDefault() ?? string.Empty,

                    TourPointDistrict = tp.TourPoint.District.Translations
                        .Where(x => x.Language.Code == culture)
                        .Select(x => x.Title)
                        .FirstOrDefault() ?? string.Empty,

                    CarBrand = tp.Vehicle.VehicleBrand.Translations
                        .Where(x => x.Language.Code == culture)
                        .Select(x => x.Title)
                        .FirstOrDefault() ?? string.Empty,

                    DriverName = tp.Driver.NameSurname ?? string.Empty,
                    TourPrice = tp.Price
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<MobileTourBookingSummaryGuideDto> TourBookingGuideSummary(
           Guid guideId, Guid districtId, Guid tourPointId, DateOnly? date
        )
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var result = await _context
                .GuideTourPrices.AsNoTracking()
                .Where(tp => tp.GuideId == guideId
                        && tp.TourPointId == tourPointId
                        && tp.DistrictId == districtId
                        && tp.Guide.Blocks.Any(a =>
                           a.StartDate == date && a.EndDate == date
                       ))
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
   Guid tourPointId, Guid cityId, Guid districtId, Guid vehicleId, decimal price)
        {

            return await _context.TourRoutePrices
               .AsNoTracking()
               .Where(tp => tp.TourPointId == tourPointId
                         && tp.VehicleId == vehicleId
                         && tp.CityId == cityId
                         && tp.DistrictId == districtId
                         && tp.Price == price
                         ).FirstOrDefaultAsync();

        }
        public async Task<AvailabilityEntity> ControlVehicleAvalibity(Guid vehicleId, DateOnly date)
        {
            return await _context.Availabilities
               .AsNoTracking()
               .Where(tp => tp.VehicleId == vehicleId
                         && !tp.BusyDays.Any(x => x.Day == date)
                         ).FirstOrDefaultAsync();
        }

        public async Task<GuideTourPriceEntity> ControlGuideAvalibity(Guid guideId, decimal price, DateOnly date, Guid tourPointId, Guid districtId, Guid cityId)
        {
            return await _context
                .GuideTourPrices.AsNoTracking()
                .Where(tp => tp.GuideId == guideId
                        && tp.TourPointId == tourPointId
                        && tp.CityId == cityId
                        && tp.DistrictId == districtId
                        && tp.Price == price
                        && !tp.Guide.Blocks.Any(a =>
                           a.StartDate == date && a.EndDate == date
                       )).FirstOrDefaultAsync();
        }



        public async Task<IEnumerable<CalendarEventDto2>> GuideEvents(FetchEventsQuery request)
        {
            // Bookings (read-only)
            var bookings = await _context
                .Bookings.Where(x =>
                    x.GuideId == request.GuidId
                    && x.Status != BookingStatus.Cancelled
                    && x.StartDate <= request.To
                    && request.From <= x.EndDate
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

        public async Task CreateGuideBlock(CreateBlockCommand request)
        {
            bool overlapsBooking = await _context.Bookings.AnyAsync(b =>
                b.GuideId == request.GuideId
                && b.Status != BookingStatus.Cancelled
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

        public async Task<Guid> FinishBooking(CreateBookingCommand request)
        {
            var entity = new BookingEntity();
            entity.CreatedDate = DateTime.Now;
            entity.CreatedAt = DateTime.Now;
            entity.CustomerId = request.CustomerId;
            entity.VehicleId = request.VehicleId;
            entity.EndDate = request.Date;
            entity.StartDate = request.Date;
            entity.FromCityId = request.CityId;
            entity.FromDistrictId = request.DistrictId;
            entity.GuideId = request.GuideId.HasValue ? request.GuideId.Value : default;
            entity.TourPointId = request.TourPointId;
            entity.TotalPrice = request.TourPrice + request.GuidePrice ?? 0;

            await _context.Bookings.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task CreateVehicleBlock(Guid vehicleId, DateOnly date)
        {
            var list = new List<BusyDayEntity>
            {
                new BusyDayEntity { Day = date }
            };
            var availability = new AvailabilityEntity
            {
                VehicleId = vehicleId,
                BusyDays = list,
            };
             _context.Availabilities.Add(availability);
            await _context.SaveChangesAsync();

        }
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
    }
}
