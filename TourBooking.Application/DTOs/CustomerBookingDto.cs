using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs
{
    public class CustomerBookingDto
    {
        public string TourPointName { get; set; } = default!;
        public string TourPointCity { get; set; }= default!;
        public string TourPointDistrict { get; set; }= default!;
        public DateOnly TourPointTime { get; set; }
        public DateOnly DepartureTime { get; set; }
        public string DriverName { get; set; }= default!;
        public string? GuideName { get; set; }
        public decimal TourPointPrice { get; set; }
        public decimal? GuidePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string VehicleBrand { get; set; }= default!;
        public int SeatCount { get; set; }
        public string DepartureLocationDescription { get; set; }= default!;
        public string DepartureCityDescription { get; set; }= default!;
        public string DepartureDistrictDescription { get; set; }= default!;

    }
}