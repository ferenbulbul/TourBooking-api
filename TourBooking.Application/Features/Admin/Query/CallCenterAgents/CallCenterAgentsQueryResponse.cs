using System;
using TourBooking.Application.DTOs.Admin;

namespace TourBooking.Application.Features.Admin.Query.AgenciesToConfirm
{
    public class CallCenterAgentsQueryResponse
    {
        public IEnumerable<CallCenterAgentDto> Agents { get; set; }
    }
}
