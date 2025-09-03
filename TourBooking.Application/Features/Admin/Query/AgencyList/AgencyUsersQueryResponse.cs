using System;
using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.Admin;

namespace TourBooking.Application.Features.Admin.Query
{
    public class AgencyUsersQueryResponse
    {
        public IEnumerable<AgencyListDto> Agencies { get; set; }
    }
}
