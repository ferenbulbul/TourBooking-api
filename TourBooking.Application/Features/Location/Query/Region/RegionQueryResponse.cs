using System;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class RegionQueryResponse
    {
        public IEnumerable<RegionDto> Regions { get; set; }
    }
}
