using System;
using TourBooking.Application.DTOs.Admin;

namespace TourBooking.Application.Features.Admin.Query.AgenciesToConfirm
{
    public class AgenciesToConfirmQueryResponse
    {
        public IEnumerable<AgencyToConfirmDto> Agencies { get; set; }
    }
}
