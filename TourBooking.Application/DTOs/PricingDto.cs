using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs
{
    public class PricingDto
    {
        public Guid? Id { get; set; }
        public Guid CountryId { get; set; }
        public Guid RegionId { get; set; }
        public Guid CityId { get; set; }
        public Guid DistrictId { get; set; }
        public Guid DriverId { get; set; }
        public Guid VehicleId { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public string VehicleName { get; set; }
        public string DriverName { get; set; }
        public decimal Price { get; set; }
    }
}
