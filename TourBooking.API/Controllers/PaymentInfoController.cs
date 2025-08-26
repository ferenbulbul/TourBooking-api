using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TourBooking.API.Controllers
{
    [ApiController]
    [Route("payment")]
    public class PaymentInfoController : ControllerBase
    {
        // merchant_ok_url -> Başarılı ödeme sonrası kullanıcı buraya gelir
        [HttpGet("success")]
        public IActionResult Success()
        {
            return Content("Ödemeniz başarıyla alınmıştır. ✅", "text/html");
        }

        // merchant_fail_url -> Başarısız ödeme sonrası kullanıcı buraya gelir
        [HttpGet("fail")]
        public IActionResult Fail()
        {
            return Content("Ödemeniz başarısız oldu. ❌", "text/html");
        }
    }
}