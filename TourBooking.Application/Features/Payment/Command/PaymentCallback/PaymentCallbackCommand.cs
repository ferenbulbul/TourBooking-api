using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TourBooking.Application.Features.Payment.Command.PaymentCallback
{
    public class PaymentCallbackCommand:IRequest<PaymentCallbackCommandResponse>
    {
        public string Token { get; set; }
    }
}