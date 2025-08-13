using System;
using TourBooking.Application.DTOs.Admin;

namespace TourBooking.Application.Features.Admin.Query.AgenciesToConfirm
{
    public class GuidesToConfirmQueryResponse
    {
        public IEnumerable<GuideToConfirmDto> Guides { get; set; }
    }
}
