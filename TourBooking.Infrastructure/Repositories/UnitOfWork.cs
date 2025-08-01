using System;
using Microsoft.EntityFrameworkCore;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
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
                    .Include(t => t.Region)
                    .ThenInclude(r => r.Translations)
                    .Include(t => t.City)
                    .ThenInclude(c => c.Translations)
                    .Include(t => t.District)
                    .ThenInclude(d => d.Translations)
                    .Include(t => t.TourDifficulty)
                    .ThenInclude(td => td.Translations)
                    .Include(t => t.TourType)
                    .ThenInclude(tt => tt.Translations)
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
    }
}
