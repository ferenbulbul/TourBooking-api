using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs
{
    public record TourRouteDto
    (
        string AgencyName,
        string TourPointName,
        string Location, // city-country-district-region
        string VehicleName,
        string DriverName,
        decimal Price,
        decimal Comission,
        DateTime CreatedAt
    );
}