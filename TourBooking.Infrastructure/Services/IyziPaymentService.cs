using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using TourBooking.Application.DTOs;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using TourBooking.Infrastructure.Repositories;

namespace TourBooking.Infrastructure.Services
{
    public class IyzicoPaymentService : Application.Interfaces.Services.IPaymentService
    {
        private readonly Iyzipay.Options _options;
        private readonly string _callbackUrl;

        public IyzicoPaymentService(IOptions<IyzicoSettings> settings)
        {
            var config = settings.Value;

            _options = new Iyzipay.Options
            {
                ApiKey = config.ApiKey,
                SecretKey = config.SecretKey,
                BaseUrl = config.BaseUrl
            };

            _callbackUrl = config.CallbackUrl;
        }


        public async Task<InitCheckoutFormDto> InitCheckoutFormAsync(BookingEntity booking, AppUser user)
        {

            var req = new CreateCheckoutFormInitializeRequest
            {
                Locale = "tr",
                ConversationId = Guid.NewGuid().ToString(),
                Price = booking.TotalPrice.ToString("F2", CultureInfo.InvariantCulture),
                PaidPrice = booking.TotalPrice.ToString("F2", CultureInfo.InvariantCulture),
                Currency = Currency.TRY.ToString(),
                BasketId = booking.TourPointId.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                CallbackUrl = _callbackUrl,
            };

            // Buyer → gerçek müşteri bilgisi gelecek
            req.Buyer = new Buyer
            {
                Id = user.Id.ToString(),
                Name = user.FirstName,
                Surname = user.LastName,
                GsmNumber = user.PhoneNumber,
                Email = user.Email,
                IdentityNumber = "74300864791",
                LastLoginDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                RegistrationDate = booking.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                RegistrationAddress = "Adres",
                Ip = "85.34.78.112",
                City = "Istanbul",
                Country = "Turkey",
                ZipCode = "34000"
            };

            // Adres
            req.ShippingAddress = new Address
            {
                ContactName = user.FirstName,
                City = "Istanbul",
                Country = "Turkey",
                Description = "Adres Detayı",
                ZipCode = "34000"
            };
            req.BillingAddress = req.ShippingAddress;

            // Basket → tek ürün olarak booking
            req.BasketItems = new List<BasketItem>
        {
            new BasketItem
            {
                Id =booking.TourPointId.ToString(), // booking.Id.ToString(),
                Name = "Tur Rezervasyonu",
                Category1 = "Tour",
                Category2 = "Booking",
                ItemType = BasketItemType.PHYSICAL.ToString(),
                Price =booking.TotalPrice.ToString("F2", CultureInfo.InvariantCulture)
            }
        };

            var init = await CheckoutFormInitialize.Create(req, _options);
            var initRaw =JsonSerializer.Serialize(init);
            if (init.Status != "success")
                throw new Exception(init.ErrorMessage ?? "Ödeme başlatılamadı");

            return new InitCheckoutFormDto
            {
                ConversationId = req.ConversationId,
                PaymentPageUrl = init.PaymentPageUrl,
                TokenExpireTime = init.TokenExpireTime,
                Token = init.Token,
                InitRaw=initRaw
            };
        }

        public async Task<PaymentResultDto> RetrieveCheckoutFormAsync(string token)
        {
            var retrieveReq = new RetrieveCheckoutFormRequest
            {
                Token = token
            };

            var result =await CheckoutForm.Retrieve(retrieveReq, _options);
            Console.WriteLine("callback başarılı");
            var RetrieveRawResponse=JsonSerializer.Serialize(result);
            return new PaymentResultDto
            {
                ConversationId = result.ConversationId,
                Token = result.Token,
                PaymentStatus = result.PaymentStatus,
                PaymentId = result.PaymentId,
                PaidPrice = decimal.TryParse(result.PaidPrice, out var paid) ? paid : 0,
                Currency = result.Currency,
                RetrieveRawResponse=RetrieveRawResponse
            };
        }
    }





}