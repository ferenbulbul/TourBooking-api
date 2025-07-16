using System;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Interfaces.Repositories
{
    public interface IVehicleRepository : IGenericRepository<VehicleType>
    {
        Task<IEnumerable<VehicleBrand>> GetAllVehicleBrandsAsync(CancellationToken cancellationToken);
    }
}
