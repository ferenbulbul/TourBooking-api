using TourBooking.Domain.Entities;

namespace TourBooking.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>()
            where T : class, IBaseEntity;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TourTypeEnitity>> TourTypes(CancellationToken cancellationToken = default);
        Task<IEnumerable<TourPointEntity>> HighlightedTourPoints(
            CancellationToken cancellationToken = default
        );
         Task<IEnumerable<TourPointEntity>> MobileTourPointByTourTypeId(
           Guid TourTypeId, CancellationToken cancellationToken = default
        );
        Task<IEnumerable<TourDifficultyEntity>> TourDifficulties(
            CancellationToken cancellationToken = default
        );
        Task<IEnumerable<VehicleType>> VehicleTypes(CancellationToken cancellationToken = default);
        Task<IEnumerable<VehicleBrand>> VehicleBrands(
            CancellationToken cancellationToken = default
        );

        Task<IEnumerable<VehicleClassEntity>> VehicleClasses(
            CancellationToken cancellationToken = default
        );
        Task<IEnumerable<SeatTypeEntity>> SeatTypes(CancellationToken cancellationToken = default);
        Task<IEnumerable<LegroomSpaceEntity>> Legrooms(
            CancellationToken cancellationToken = default
        );
        Task<IEnumerable<CityEntity>> Cities(CancellationToken cancellationToken = default);
        Task<CityEntity> City(Guid Id, CancellationToken cancellationToken = default);
        Task<IEnumerable<CountryEntity>> Countries(CancellationToken cancellationToken = default);
        Task<CountryEntity> Country(Guid Id, CancellationToken cancellationToken = default);
        Task<IEnumerable<RegionEntity>> Regions(CancellationToken cancellationToken = default);
        Task<RegionEntity> Region(Guid Id, CancellationToken cancellationToken = default);
        Task<IEnumerable<DistrictEntity>> Districts(CancellationToken cancellationToken = default);
        Task<DistrictEntity> District(Guid Id, CancellationToken cancellationToken = default);
        Task<VehicleType> VehicleType(Guid Id, CancellationToken cancellationToken = default);
        Task<VehicleBrand> VehicleBrand(Guid Id, CancellationToken cancellationToken = default);
        Task<SeatTypeEntity> SeatType(Guid Id, CancellationToken cancellationToken = default);
        Task<VehicleClassEntity> VehicleClass(
            Guid Id,
            CancellationToken cancellationToken = default
        );
        Task<LegroomSpaceEntity> Legroom(Guid Id, CancellationToken cancellationToken = default);
        Task<TourDifficultyEntity> TourDifficulty(
            Guid Id,
            CancellationToken cancellationToken = default
        );
        Task<TourTypeEnitity> TourType(Guid Id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TourPointEntity>> TourPoints(
            CancellationToken cancellationToken = default
        );
        Task<TourPointEntity> TourPoint(Guid Id, CancellationToken cancellationToken = default);
        Task<DriverEntity> Driver(Guid Id, CancellationToken cancellationToken = default);
        Task<VehicleEntity> Vehicle(Guid Id, CancellationToken cancellationToken = default);
        Task<AvailabilityEntity> VehicleAvailabilityByVehicleId(
            Guid vehicleId,
            CancellationToken cancellationToken = default
        );
        Task<IEnumerable<AvailabilityEntity>> VehicleAvailabilities(
            CancellationToken cancellationToken = default
        );
        Task<IEnumerable<DriverEntity>> DriversForAgency(
            Guid agencyId,
            CancellationToken cancellationToken = default
        );
        Task<IEnumerable<DriverEntity>> Drivers(CancellationToken cancellationToken = default);
        Task<IEnumerable<VehicleEntity>> Vehicles(CancellationToken cancellationToken = default);
        Task<IEnumerable<VehicleEntity>> VehiclesForAgency(
            Guid agencyId,
            CancellationToken cancellationToken = default
        );
        Task<TourEntity> Tour(Guid Id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TourEntity>> Tours(CancellationToken cancellationToken = default);
        Task<IEnumerable<TourEntity>> ToursForAgency(Guid agencyId, CancellationToken cancellationToken = default);
    }
}
