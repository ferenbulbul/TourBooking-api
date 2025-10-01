using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs
{
    public class InitCheckoutFormDto
    {
        public string ConversationId { get; set; } = "";
        public string PaymentPageUrl { get; set; } = "";
        public string Token { get; set; } = "";
        public long? TokenExpireTime { get; set; }
        public string InitRaw { get; set; } = "";

    }
}