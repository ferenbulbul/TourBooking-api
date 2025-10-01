using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Payment.Command.PaymentCallback
{
    public class PaymentCallbackCommandResponse
    {
        public string ConversationId { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string PaymentStatus { get; set; } = default!; // SUCCESS / FAILURE
        public string? PaymentId { get; set; }
        public decimal PaidPrice { get; set; }
        public string Currency { get; set; } = "TRY";
    }
}