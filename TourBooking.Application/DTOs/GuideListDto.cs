using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs
{
    public record GuideListDto
    (
        Guid Id,
        string FullName,
        string Email,
        string Phone,
        string Domestic,
        string Regional,
        string Photo,
        DateTime CreatedAt
    );

}