using System;

namespace TourBooking.Application.DTOs
{
    public class VehicleBrandDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
