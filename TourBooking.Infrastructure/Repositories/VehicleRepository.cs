using System;
using Microsoft.EntityFrameworkCore;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Infrastructure.Context;

namespace TourBooking.Infrastructure.Repositories
{
    public class VehicleRepository : GenericRepository<VehicleType>, IVehicleRepository
    {
        public VehicleRepository(AppDbContext context)
            : base(context) { }

        public async Task<IEnumerable<VehicleBrand>> GetAllVehicleBrandsAsync(
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result = await _context
                    .VehicleBrands.Where(v => !v.IsDeleted && v.IsActive)
                    .ToListAsync(cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                // Hata logla ya da response'a yaz
                throw new Exception("VehicleBrands sorgusunda hata oluştu: " + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<VehicleType>> GetAllVehicleTypesAsync(
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result = await _context
                    .VehicleTypes.Where(v =>
                        (v.IsDeleted == false || v.IsDeleted == null) && v.IsActive == true
                    )
                    .ToListAsync(cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                // Hata logla ya da response'a yaz
                throw new Exception("VehicleTypes sorgusunda hata oluştu: " + ex.Message, ex);
            }
        }
    }
}
