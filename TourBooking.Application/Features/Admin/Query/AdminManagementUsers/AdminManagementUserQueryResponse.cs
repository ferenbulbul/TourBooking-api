using System;
using TourBooking.Application.DTOs.Admin;

namespace TourBooking.Application.Features.Admin.Query.AgenciesToConfirm
{
    public class AdminManagementUserQueryResponse
    {
        public IEnumerable<AdminManagementUserDto> Users { get; set; }
    }
}
