using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Threading.Tasks;
using TourBooking.API;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Features;
using TourBooking.Application.Features.Payment.Command;
using TourBooking.Application.Features.Payment.Command.PaymentCallback;

namespace Payments.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : BaseController
{
    private readonly IMediator _mediator;
    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [Authorize]
    [HttpPost("cf-init")]
    public async Task<IActionResult> InitCheckoutForm([FromBody] InitCheckoutFormRequest request)
    {

        var userId = GetUserIdFromToken();
        var command = new InitCheckoutFormCommand { UserId = userId, BookingId = request.BookingId };
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<InitCheckoutFormCommandResponse>.SuccessResponse(result,"başarıllı"));
    }
    public class InitCheckoutFormRequest
    {
        public Guid BookingId { get; set; }
    }

    // STEP 1: Checkout Form Initialize
    // [HttpPost("cf-init")]
    // public async Task<IActionResult> InitCheckoutForm([FromBody] InitCheckoutFormRequest request)
    // {
    //     var req = new CreateCheckoutFormInitializeRequest
    //     {
    //         Locale = "tr",
    //         ConversationId = Guid.NewGuid().ToString(),
    //         Price = "6.0",
    //         PaidPrice = "6.0",
    //         Currency = Currency.TRY.ToString(),
    //         BasketId = "B67832",
    //         PaymentGroup = PaymentGroup.PRODUCT.ToString(),
    //         CallbackUrl = _callbackUrl
    //     };

    //     // Buyer
    //     req.Buyer = new Buyer
    //     {
    //         Id = "BY789",
    //         Name = "John",
    //         Surname = "Doe",
    //         GsmNumber = "+905350000000",
    //         Email = "sandboxtest@email.com",
    //         IdentityNumber = "74300864791",
    //         LastLoginDate = "2015-10-05 12:43:35",
    //         RegistrationDate = "2013-04-21 15:12:09",
    //         RegistrationAddress = "Altunizade Mah. İnci Çıkmazı Sokak No: 3 İç Kapı No: 10 Üsküdar İstanbul",
    //         Ip = "85.34.78.112",
    //         City = "Istanbul",
    //         Country = "Turkey",
    //         ZipCode = "34732"
    //     };

    //     // Address
    //     req.ShippingAddress = new Address
    //     {
    //         ContactName = "Jane Doe",
    //         City = "Istanbul",
    //         Country = "Turkey",
    //         Description = "Altunizade Mah. İnci Çıkmazı Sokak No: 3 İç Kapı No: 10 Üsküdar İstanbul",
    //         ZipCode = "34742"
    //     };

    //     req.BillingAddress = req.ShippingAddress;

    //     // Basket
    //     req.BasketItems = new List<BasketItem>
    //     {
    //         new BasketItem
    //         {
    //             Id = "BI101",
    //             Name = "Binocular",
    //             Category1 = "Collectibles",
    //             Category2 = "Accessories",
    //             ItemType = BasketItemType.PHYSICAL.ToString(),
    //             Price = "1.0"
    //         },
    //         new BasketItem
    //         {
    //             Id = "BI102",
    //             Name = "Game code",
    //             Category1 = "Game",
    //             Category2 = "Online Game Items",
    //             ItemType = BasketItemType.VIRTUAL.ToString(),
    //             Price = "2.0"
    //         },
    //         new BasketItem
    //         {
    //             Id = "BI103",
    //             Name = "Usb",
    //             Category1 = "Electronics",
    //             Category2 = "Usb / Cable",
    //             ItemType = BasketItemType.PHYSICAL.ToString(),
    //             Price = "3.0"
    //         }
    //     };

    //     var init = await CheckoutFormInitialize.Create(req, _options);
    //     if (init.Status != "success")
    //     {
    //         Console.WriteLine(init.ErrorMessage);
    //         return BadRequest(ApiResponse<InitCheckoutFormDto>.FailResponse(init.ErrorMessage ?? "Ödeme başlatılamadı"));
    //     }

    //     // Mobil için minimal response
    //     var dto = new InitCheckoutFormDto
    //     {
    //         ConversationId = req.ConversationId,
    //         PaymentPageUrl = init.PaymentPageUrl,
    //         TokenExpireTime = init.TokenExpireTime
    //     };

    //     // TODO: conversationId ve token’ı DB’ye kaydet (order tablosuna)
    //     // çünkü callback geldiğinde eşleşmeye ihtiyacın olacak

    //     return Ok(ApiResponse<InitCheckoutFormDto>.SuccessResponse(dto, null));

    //     //return Ok(init);
    // }

    //STEP 2: Callback (iyzico ödeme bitince burayı POST eder)
    [HttpPost("callback")]
    public async Task<IActionResult> Callback([FromForm] string token)
    {
        Console.WriteLine(token);
        var command = new PaymentCallbackCommand{Token=token};
        var result=await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("result")]
    public async Task<IActionResult> GetPaymentResult(string conversationId)
    {
        var a = conversationId;
        // DB'den bu conversationId'ye ait token'ı bul
        // (cf-init çağrısında kaydetmen lazım)
        // var paymentToken = await _dbContext.Payments
        //     .Where(p => p.ConversationId == conversationId)
        //     .Select(p => p.Token)
        //     .FirstOrDefaultAsync();

        // if (paymentToken == null)
        // {
        //     return NotFound(ApiResponse<PaymentResultResponse>.FailResponse(
        //         "ConversationId bulunamadı", 404));
        // }

        // iyzico'dan sonucu sorgula
        // var request = new RetrieveCheckoutFormRequest
        // {
        //     Token = paymentToken
        // };

        // var checkoutForm = await CheckoutForm.Retrieve(request, _options);

        // if (checkoutForm == null)
        // {
        //     return BadRequest(ApiResponse<PaymentResultResponse>.FailResponse(
        //         "Sonuç alınamadı", 400));
        // }

        // Response DTO'ya map et
        // var result = new PaymentResultResponse
        // {
        //     ConversationId = checkoutForm.ConversationId,
        //     PaymentStatus = checkoutForm.PaymentStatus, // SUCCESS / FAILURE
        //     PaymentId = checkoutForm.PaymentId,
        //     BasketId = checkoutForm.BasketId,
        //     Price = checkoutForm.Price,
        //     PaidPrice = checkoutForm.PaidPrice,
        //     ErrorMessage = checkoutForm.ErrorMessage
        // };
        var result = new PaymentResultResponse
        {
            ConversationId = "checkoutForm.ConversationId",
            PaymentStatus = "SUCCESS", // SUCCESS / FAILURE
            PaymentId = "checkoutForm.PaymentId",
            Price = "checkoutForm.Price",
            PaidPrice = "checkoutForm.PaidPrice",
            ErrorMessage = "checkoutForm.ErrorMessage"
        };

        return Ok(ApiResponse<PaymentResultResponse>.SuccessResponse(result, null));
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
public class PaymentResultResponse
{
    public string ConversationId { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty; // SUCCESS / FAILURE
    public string? PaymentId { get; set; }
    public string? Price { get; set; }
    public string? PaidPrice { get; set; }
    public string? ErrorMessage { get; set; }
}