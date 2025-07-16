using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.Expactions
{
    public class NotFoundException : ApplicationException
    {
        /// <summary>
        /// HTTP 404 Not Found durum koduna karşılık gelir.
        /// </summary>
        public NotFoundException(string message) : base(message)
        {
        }
    }
}