using System;
using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features
{
    public class CreateBookingCommandResponse
    {
        public bool IsValid { get; set; }
        public Guid BookingId { get; set; }
    }
}
