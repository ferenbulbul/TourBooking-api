using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.Expactions
{
    /// <summary>
    /// Bir iş kuralı ihlal edildiğinde fırlatılır.
    /// HTTP 409 Conflict veya 400 Bad Request durum koduna karşılık gelebilir.
    /// </summary>
    public class BusinessRuleValidationException : ApplicationException
    {
        public BusinessRuleValidationException(string message) : base(message)
        {
        }
    }
}