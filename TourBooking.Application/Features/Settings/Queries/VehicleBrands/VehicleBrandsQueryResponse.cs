using System;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Vehicle.Queries.VehicleBrands
{
    public class VehicleBrandsQueryResponse
    {
        public IEnumerable<VehicleBrandDto> VehicleBrands { get; set; }
    }
}
