using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
    public class IyzicoSettings
    {
        public string ApiKey { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public string BaseUrl { get; set; } = default!;
        public string CallbackUrl { get; set; } = default!;
    }
}