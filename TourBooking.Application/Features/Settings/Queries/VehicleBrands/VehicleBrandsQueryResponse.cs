using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class VehicleBrandsQueryResponse
    {
        public IEnumerable<VehicleBrandDto> VehicleBrands { get; set; }
    }
}
