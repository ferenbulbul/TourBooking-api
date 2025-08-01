using System;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class VehicleClassQueryResponse
    {
        public IEnumerable<VehicleClassDto> VehicleClasses { get; set; }
    }
}
