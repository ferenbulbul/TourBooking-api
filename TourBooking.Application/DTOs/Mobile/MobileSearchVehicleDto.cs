using System;

namespace TourBooking.Application.DTOs.Mobile
{
    public class MobileSearchVehicleDto
    {
        public Guid VehicleId { get; set; }
        public decimal Price { get; set; }
        public string VehicleBrand { get; set; } = string.Empty;
        public string VehicleClass { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public int SeatCount { get; set; }
        public string? Image { get; set; }
    }
}
