using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TourBooking.Application.Features.Payment.Command
{
    public class InitCheckoutFormCommand : IRequest<InitCheckoutFormCommandResponse>
    {
        public Guid UserId { get; set; }
        public Guid BookingId { get; set; }
    }

}