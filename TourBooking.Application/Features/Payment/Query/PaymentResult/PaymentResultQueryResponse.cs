using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.Features.Payment.Query.PaymentResult
{
    public class PaymentResultQueryResponse
    {
        public string PaymentStatus { get; set; } = default!;
        public string ConversationId { get; set; } = default!;

    }
}
            // ConversationId = "checkoutForm.ConversationId",
            // PaymentStatus = "SUCCESS", // SUCCESS / FAILURE
            // PaymentId = "checkoutForm.PaymentId",
            // Price = "checkoutForm.Price",
            // PaidPrice = "checkoutForm.PaidPrice",
            // ErrorMessage = "checkoutForm.ErrorMessage"