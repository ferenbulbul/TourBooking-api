using System;
using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.Admin;

namespace TourBooking.Application.Features.Admin.Query
{
    public class VehicleListQueryResponse
    {
        public IEnumerable<VehicleListDto> Vehicles { get; set; }
    }
}
