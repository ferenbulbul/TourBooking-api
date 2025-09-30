using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.Features
{
    public class InitCheckoutFormCommandResponse
    {
        public string ConversationId { get; set; } = null!;
        public string PaymentPageUrl { get; set; } = null!;
        public long? TokenExpireTime { get; set; }
    }
}