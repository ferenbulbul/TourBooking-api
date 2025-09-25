using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Threading.Tasks;
using TourBooking.Application.DTOs.Comman;

namespace Payments.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly Options _options;
    private readonly string _callbackUrl;

    public PaymentsController(IConfiguration config)
    {
        _options = new Options
        {
            ApiKey = config["Iyzico:ApiKey"],
            SecretKey = config["Iyzico:SecretKey"],
            BaseUrl = config["Iyzico:BaseUrl"]
        };
        _callbackUrl = config["Iyzico:CallbackUrl"]!;
    }

    // STEP 1: Checkout Form Initialize
    [HttpPost("cf-init")]
    public async Task<IActionResult> InitCheckoutForm([FromBody] InitCheckoutFormRequest request)
    {
        var req = new CreateCheckoutFormInitializeRequest
        {
            Locale = "tr",
            ConversationId = Guid.NewGuid().ToString(),
            Price = "6.0",
            PaidPrice = "6.0",
            Currency = Currency.TRY.ToString(),
            BasketId = "B67832",
            PaymentGroup = PaymentGroup.PRODUCT.ToString(),
            CallbackUrl = _callbackUrl
        };

        // Buyer
        req.Buyer = new Buyer
        {
            Id = "BY789",
            Name = "John",
            Surname = "Doe",
            GsmNumber = "+905350000000",
            Email = "sandboxtest@email.com",
            IdentityNumber = "74300864791",
            LastLoginDate = "2015-10-05 12:43:35",
            RegistrationDate = "2013-04-21 15:12:09",
            RegistrationAddress = "Altunizade Mah. İnci Çıkmazı Sokak No: 3 İç Kapı No: 10 Üsküdar İstanbul",
            Ip = "85.34.78.112",
            City = "Istanbul",
            Country = "Turkey",
            ZipCode = "34732"
        };

        // Address
        req.ShippingAddress = new Address
        {
            ContactName = "Jane Doe",
            City = "Istanbul",
            Country = "Turkey",
            Description = "Altunizade Mah. İnci Çıkmazı Sokak No: 3 İç Kapı No: 10 Üsküdar İstanbul",
            ZipCode = "34742"
        };

        req.BillingAddress = req.ShippingAddress;

        // Basket
        req.BasketItems = new List<BasketItem>
        {
            new BasketItem
            {
                Id = "BI101",
                Name = "Binocular",
                Category1 = "Collectibles",
                Category2 = "Accessories",
                ItemType = BasketItemType.PHYSICAL.ToString(),
                Price = "1.0"
            },
            new BasketItem
            {
                Id = "BI102",
                Name = "Game code",
                Category1 = "Game",
                Category2 = "Online Game Items",
                ItemType = BasketItemType.VIRTUAL.ToString(),
                Price = "2.0"
            },
            new BasketItem
            {
                Id = "BI103",
                Name = "Usb",
                Category1 = "Electronics",
                Category2 = "Usb / Cable",
                ItemType = BasketItemType.PHYSICAL.ToString(),
                Price = "3.0"
            }
        };

        var init =await CheckoutFormInitialize.Create(req, _options);

        if (init.Status != "success")
            return BadRequest(ApiResponse<InitCheckoutFormDto>.FailResponse(init.ErrorMessage ?? "Ödeme başlatılamadı"));

        // Mobil için minimal response
        var dto = new InitCheckoutFormDto
        {
            ConversationId = req.ConversationId,
            PaymentPageUrl = init.PaymentPageUrl,
            TokenExpireTime = init.TokenExpireTime
        };

        // TODO: conversationId ve token’ı DB’ye kaydet (order tablosuna)
        // çünkü callback geldiğinde eşleşmeye ihtiyacın olacak

        return Ok(ApiResponse<InitCheckoutFormDto>.SuccessResponse(dto, null));

        //return Ok(init);
    }

    // STEP 2: Callback (iyzico ödeme bitince burayı POST eder)
    [HttpPost("callback")]
    public IActionResult Callback([FromForm] string token)
    {
        var retrieveReq = new RetrieveCheckoutFormRequest
        {
            Token = token
        };

        var result = CheckoutForm.Retrieve(retrieveReq, _options);
        Console.WriteLine("callback başarılı"+result);
        // burada DB güncelleme yapabilirsin
        // result.PaymentStatus = "SUCCESS" ise siparişi başarılı işaretle

        return Ok(result);
    }
}
public class InitCheckoutFormDto
{
    public string ConversationId { get; set; } = "";
    public string PaymentPageUrl { get; set; } = "";
    public long? TokenExpireTime { get; set; }
}
public class InitCheckoutFormRequest
{
    public string BookingId { get; set; }
}
