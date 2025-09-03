using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs
{
    public record VehicleListDto
    (
        Guid Id,
        string AgencyName,
        string VehicleName,
        string TypeName,
        string BrandName,
        string ClassName,
        int SeatCount,
        string Plate,
        int Year,
        string Picture,
        string Ruhsat,
        string Sigorta,
        string Tasimacilik
    );
}