using System;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class DistrictQueryResponse
    {
        public IEnumerable<DistrictDto> Districts { get; set; }
    }
}
