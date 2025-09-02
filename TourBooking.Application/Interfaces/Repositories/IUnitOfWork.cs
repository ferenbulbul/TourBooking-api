using Microsoft.EntityFrameworkCore.Storage;
using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.Admin;
using TourBooking.Application.DTOs.GuideCalendar;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Features;
using TourBooking.Application.Features.Authentication.Queries.IsApproved;
using TourBooking.Application.Features.Settings;
using TourBooking.Application.Features.Settings.Queries;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>()
            where T : class, IBaseEntity;
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> CommitAsync();
        Task RollbackAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TourTypeEnitity>> TourTypes(CancellationToken cancellationToken = default);
        Task<IEnumerable<MobileHighlightedTourPointDto>> HighlightedTourPoints(
            CancellationToken cancellationToken = default
        );
        Task<IEnumerable<MobileTourPointsBySearchDto>> MobileTourPointBySearch(
            string searchQuery,
            string culture,
            CancellationToken cancellationToken = default
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
        Task<IEnumerable<MobileCityDto>> CitiesForMobile(
            Guid regionId,
            string culture,
            CancellationToken cancellationToken = default
        );
        Task<IEnumerable<MobileRegionDto>> RegionsForMobile(
            string culture,
            CancellationToken cancellationToken = default
        );
        Task<IEnumerable<TourEntity>> ToursForAgency(
            Guid agencyId,
            CancellationToken cancellationToken = default
        );
        Task<IEnumerable<MobileDistrictDto>> DistrictsForMobile(Guid cityId, string culture);
        Task<IEnumerable<MobileDetailedSearchResultDto>> MobileTourPointsByLocation(
            Guid? regionId,
            Guid? cityId,
            Guid? districtId,
            string culture
        );
        Task<IEnumerable<MobileDetailedSearchResultDto>> MobileTourPointsByDeparture(
            Guid? regionId,
            Guid? cityId,
            Guid? districtId,
            string culture
        );

        Task<MobileTourPointDetailDto> MobileTourPointDetail(Guid tourPointId, string culture, Guid userId);
        Task<IEnumerable<MobileSearchVehicleDto>> MobileSearchVehicles(
            MobileSearchVehiclesQuery request
        );
        Task<IEnumerable<MobileSearchGuidesDto>> MobileSearchGuides(MobileSearchGuideQuery request);
        Task<IEnumerable<CalendarEventDto2>> GuideEvents(FetchEventsQuery request);
        Task CreateGuideBlock(CreateBlockCommand request);

        Task RemoveGuideBlock(RemoveBlockCommand request);
        Task<MobileDetailVehicleDto> MobileDetailVehicle(Guid vehicleId);
        Task<MobileTourBookingSummaryVehicleTourDto> TourBookingVehicleTourSummary(
            Guid tourPointId,
            Guid DistrictId,
            Guid VehicleId,
            DateOnly? date
        );
        Task<MobileTourBookingSummaryGuideDto> TourBookingGuideSummary(
            Guid guideId,
            Guid districtId,
            Guid tourPointId,
            DateOnly? date
        );
        Task<TourRoutePriceEntity> ControlTourRoute(
            Guid tourPointId,
            Guid cityId,
            Guid districtId,
            Guid vehicleId,
            decimal price
        );
        Task<VehicleBlockEntity> ControlVehicleAvalibity(Guid vehicleId, DateOnly date);
        Task<GuideTourPriceEntity> ControlGuideAvalibity(
            Guid guideId,
            decimal price,
            DateOnly date,
            Guid tourPointId,
            Guid districtId,
            Guid cityId
        );
        Task<Guid> FinishBooking(CreateBookingCommand request, Guid driverId, Guid agencyId);
        //Task CreateVehicleBlock(Guid vehicleId, DateOnly date);
        Task<bool> IsUserApproved(IsApprovedQuery request);
        Task<IEnumerable<AgencyToConfirmDto>> GetAgenciesToConfirm();
        Task<IEnumerable<GuideToConfirmDto>> GetGuidesToConfirm();
        Task ConfirmGuide(Guid ıd);
        Task ConfirmAgency(Guid ıd);
        Task CreateVehicleBlock(CreateVehicleBlockCommand request);
        Task RemoveVehicleBlock(RemoveVehicleBlockCommand request);
        Task<IEnumerable<CalendarEventDto2>> VehicleEvents(FetchVehicleEventsQuery request);
        Task<IEnumerable<MobileDetailedSearchResultDto>> MobileTourPointByTourTypeId(Guid? tourType, string culture);
        Task<List<DriverLocationDto>> DriverLocations();
        Task<SystemCountDto> SystemCounts();
        Task<FavoriteEntity> ToggleFavoriteAsync(Guid customerId, Guid tourPointId);
        Task<IEnumerable<MobileHighlightedTourPointDto>> CustomerFavorites(Guid customerId,
            CancellationToken cancellationToken = default
        );
    }
}
