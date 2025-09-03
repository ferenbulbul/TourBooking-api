using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs
{
    public record GuideTourDto
    (
        string GuideName,
        string Location, //city-district-
        string TourPointName,
        decimal Price,
        decimal Commission,
        DateTime CreatedAt

    );

    
}