using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.DTOs.Mobile
{
    public class MobileDetailVehicleDto
    {

        public string VehicleType { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleClass { get; set; }
        public string LegRoomSpace { get; set; }
        public string SeatType { get; set; }
        public int SeatCount { get; set; }
        public int ModelYear { get; set; }
        public string Image { get; set; }
        public List<string>? OtherImages { get; set; }
        public List<string>? VehicleFeatures { get; set; }
    }
}