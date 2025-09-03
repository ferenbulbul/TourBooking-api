using System;
using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.Admin;

namespace TourBooking.Application.Features.Admin.Query
{
    public class CustomerUserQueryResponse
    {
        public IEnumerable<CustomerUserDto> Customers { get; set; }
    }
}
