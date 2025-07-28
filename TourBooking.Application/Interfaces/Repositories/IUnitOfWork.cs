using System;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>()
            where T : class, IBaseEntity;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TourTypeEnitity>> TourTypes(CancellationToken cancellationToken = default);
        Task<IEnumerable<TourDifficultyEntity>> TourDifficulties(CancellationToken cancellationToken = default);
        Task<IEnumerable<VehicleType>> VehicleTypes(CancellationToken cancellationToken = default);
        Task<VehicleType> VehicleType(Guid Id, CancellationToken cancellationToken = default);
        Task<TourDifficultyEntity> TourDifficulty(Guid Id, CancellationToken cancellationToken = default);
        Task<TourTypeEnitity> TourType(Guid Id,CancellationToken cancellationToken = default);
    }
}
