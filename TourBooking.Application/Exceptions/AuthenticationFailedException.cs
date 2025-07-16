using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.Expactions
{
      /// <summary>
    /// Kullanıcı kimlik doğrulaması başarısız olduğunda fırlatılır.
    /// HTTP 401 Unauthorized durum koduna karşılık gelir.
    /// </summary>
    public class AuthenticationFailedException : ApplicationException
    {
        public AuthenticationFailedException(string message) : base(message)
        {
        }
    }
}