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
    }
}
