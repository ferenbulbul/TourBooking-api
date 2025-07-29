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
        Task<IEnumerable<CityEntity>> Cities(CancellationToken cancellationToken = default);
        Task<CityEntity> City(Guid Id, CancellationToken cancellationToken = default);
        Task<IEnumerable<CountryEntity>> Countries(CancellationToken cancellationToken = default);
        Task<CountryEntity> Country(Guid Id, CancellationToken cancellationToken = default);
        Task<IEnumerable<RegionEntity>> Regions(CancellationToken cancellationToken = default);
        Task<RegionEntity> Region(Guid Id, CancellationToken cancellationToken = default);
        Task<IEnumerable<DistrictEntity>> Districts(CancellationToken cancellationToken = default);
        Task<DistrictEntity> District(Guid Id, CancellationToken cancellationToken = default);
        Task<VehicleType> VehicleType(Guid Id, CancellationToken cancellationToken = default);
        Task<TourDifficultyEntity> TourDifficulty(Guid Id, CancellationToken cancellationToken = default);
        Task<TourTypeEnitity> TourType(Guid Id,CancellationToken cancellationToken = default);
    }
}
