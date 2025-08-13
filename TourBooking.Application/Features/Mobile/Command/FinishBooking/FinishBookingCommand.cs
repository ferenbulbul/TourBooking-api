using System;
using MediatR;

namespace TourBooking.Application.Features
{
    public class FinishBookingCommand : IRequest<FinishBookingCommandResponse>
    {
        public Guid BookingId { get; set; }
    }
}
