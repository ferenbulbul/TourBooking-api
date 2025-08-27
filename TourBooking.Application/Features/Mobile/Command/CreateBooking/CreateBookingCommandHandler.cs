using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Features.Mobile.Query.TourPointDetails;
using TourBooking.Application.Features.Settings;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class CreateBookingCommandHandler
    : IRequestHandler<CreateBookingCommand, CreateBookingCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateBookingCommandResponse> Handle(
        CreateBookingCommand request,
        CancellationToken cancellationToken
    )
    {

        var tourControl = await _unitOfWork.ControlTourRoute(request.TourPointId, request.CityId, request.DistrictId, request.VehicleId, request.TourPrice);
        if (tourControl == null)
        {
            throw new BusinessRuleValidationException("tur kontrol fayil");
        }
        var vehicleControl = await _unitOfWork.ControlVehicleAvalibity(request.VehicleId, request.Date);
        if (vehicleControl != null)
        {
            throw new BusinessRuleValidationException("araç kontrol fayil");
        }
        if (request.GuideId is not null && request.GuidePrice is not null)
        {
            var guideControl = await _unitOfWork.ControlGuideAvalibity(request.GuideId.Value, request.GuidePrice.Value, request.Date, request.TourPointId, request.DistrictId, request.CityId);
            if (guideControl == null)
            {
                throw new BusinessRuleValidationException("rehberss kontrol fayil");
            }
        }
        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            // 1️⃣ Booking ekle
            var bookingId = await _unitOfWork.FinishBooking(request, tourControl.DriverId, tourControl.AgencyId);
            if (request.GuideId.HasValue)
            {
                await _unitOfWork.CreateGuideBlock(new CreateBlockCommand { GuideId = request.GuideId.Value, Start = request.Date, End = request.Date, Note = bookingId.ToString() });
            }

            await _unitOfWork.CreateVehicleBlock(new CreateVehicleBlockCommand { VehicleId = request.VehicleId, Start = request.Date, End = request.Date, Note = bookingId.ToString() });


            await _unitOfWork.CommitAsync();
            await transaction.CommitAsync();

            var iframeUrlParameters = GetPayTRToken(request, bookingId);
            

            return new CreateBookingCommandResponse { BookingId = bookingId, IsValid = true };
        }
        catch
        {
            // Hata olursa rollback
            await transaction.RollbackAsync();
            throw new BusinessRuleValidationException("Rezervasyon oluşmadı ödemeyi bi ara alırsın");
        }
    }

    private async Task<string> GetPayTRToken(CreateBookingCommand request, Guid bookingId)
    {
        // getEmailByUserId;
        // amount = request.TourPrice + (request.GuidePrice ?? 0);
        // paytrAmount = amount * 100;
        // sepet hazırla
        // no_installment = 1
        // max_installment = 0
        // paytr_token hashaleme olayı
        // user_name getUserNameByUserId
        // user_address 
        // user_phone = GetPhoneByUserId
        // merchant_ok_url -> https://tourbooking-api-272954735037.europe-west2.run.app/payment/success
        // merchant_fail_url -> https://tourbooking-api-272954735037.europe-west2.run.app/payment/fail
        // test_mode = 1
        // debug_on = 1
        // ####################### DÜZENLEMESİ ZORUNLU ALANLAR #######################
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
        string basketJson = JsonSerializer.Serialize(user_basket);
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
        var response = await client.PostAsync("https://www.paytr.com/odeme/api/get-token", content);
        var result = await response.Content.ReadAsStringAsync();

        return result;

    }
}
