using System;
using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.Admin;

namespace TourBooking.Application.Features.Admin.Query
{
    public class GuideTourRouteQueryResponse
    {
        public IEnumerable<GuideTourDto> Routes { get; set; }
    }
}
