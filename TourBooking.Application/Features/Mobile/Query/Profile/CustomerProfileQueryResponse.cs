using System;

namespace TourBooking.Application.Features
{
    public class CustomerProfileQueryResponse
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
    }
}
