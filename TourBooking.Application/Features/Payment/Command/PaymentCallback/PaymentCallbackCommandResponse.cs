using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Payment.Command.PaymentCallback
{
    public class PaymentCallbackCommandResponse
    {
        public PaymentResultDto paymentResultDto { get; set; }
    }
}