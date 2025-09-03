using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs
{
    public record AgencyListDto
    (
        Guid Id,
        string FullName,
        string Adress,
        string Location,
        string CompanyName,
        string Email,
        string Phone1,
        string Phone2,
        string Tax,
        string Tursab,
        DateTime CreatedAt
    );
}