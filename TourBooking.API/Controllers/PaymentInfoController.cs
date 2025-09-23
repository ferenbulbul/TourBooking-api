using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TourBooking.Application.Expactions;

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

        [HttpGet("pay-test")]
        public async Task<IActionResult> PayTest()
        {
            string merchant_id = "141001";
            string merchant_key = "1TtYcusi16g2dPzZ";
            string merchant_salt = "1gXug4RiPUzByzZC";

            string email = "e176demir@gmail.com";
            int payment_amount = 999; // Örn: 9.99 TL için 999 gönderilir
            string merchant_oid = Guid.NewGuid().ToString();
            string user_name = "Ali Veli";
            string user_address = "İstanbul, Türkiye";
            string user_phone = "5555555555";

            string merchant_ok_url = "https://tourbooking-api-272954735037.europe-west2.run.app/payment/success";
            string merchant_fail_url = "https://tourbooking-api-272954735037.europe-west2.run.app/payment/fail";

            string user_ip = "127.0.0.1";

            var user_basket = new object[][]
            {
                new object[] { "Örnek ürün 1", "999", 1 },
            };

            string timeout_limit = "30";
            string debug_on = "1";
            string test_mode = "1";
            string no_installment = "0";
            string max_installment = "0";
            string currency = "TL";
            string lang = "tr";

            // ####################### TOKEN OLUŞTURMA #######################
            string basketJson = System.Text.Json.JsonSerializer.Serialize(user_basket);
            string user_basket_str = Convert.ToBase64String(Encoding.UTF8.GetBytes(basketJson));

            string concat = string.Concat(
                merchant_id, user_ip, merchant_oid, email, payment_amount.ToString(),
                user_basket_str, no_installment, max_installment, currency, test_mode, merchant_salt
            );

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchant_key));
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(concat));
            string paytr_token = Convert.ToBase64String(hash);

            using var client = new HttpClient();
            var dict = new Dictionary<string, string>
            {
                { "merchant_id", merchant_id },
                { "user_ip", user_ip },
                { "merchant_oid", merchant_oid },
                { "email", email },
                { "payment_amount", payment_amount.ToString() },
                { "user_basket", user_basket_str },
                { "paytr_token", paytr_token },
                { "debug_on", debug_on },
                { "test_mode", test_mode },
                { "no_installment", no_installment },
                { "max_installment", max_installment },
                { "user_name", user_name },
                { "user_address", user_address },
                { "user_phone", user_phone },
                { "merchant_ok_url", merchant_ok_url },
                { "merchant_fail_url", merchant_fail_url },
                { "timeout_limit", timeout_limit },
                { "currency", currency },
                { "lang", lang }
            };
            var content = new FormUrlEncodedContent(dict);
            try
            {
                var response = await client.PostAsync("https://www.paytr.com/odeme/api/get-token", content);
                if (response != null && response.StatusCode != HttpStatusCode.OK)
                {
                    throw new BusinessRuleValidationException(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<NetgsmResponse>(responseString);
                    if (result != null && result.Status == "failed")
                    {
                        throw new BusinessRuleValidationException(string.Concat("Ödeme alınırken sorun oluştu -> ", result.Reason));
                    }
                    if (result != null && result.Status != "failed")
                    {
                        // merchant_oid üzerinden kendi tablonu ödeme alındı olarak ghüncelle
                    }


                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Content("Ödemeniz başarısız oldu. ❌", "text/html");
        }


    }
    public class NetgsmResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

}