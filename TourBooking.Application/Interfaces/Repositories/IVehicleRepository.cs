using System;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Interfaces.Repositories
{
    public interface IVehicleRepository 
    {
        Task<IEnumerable<VehicleBrand>> GetAllVehicleBrandsAsync(CancellationToken cancellationToken);
    }
}
