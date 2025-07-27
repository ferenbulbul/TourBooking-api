using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using TourBooking.Application.Features.Authentication.Commands.ForgotPassword;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;

public class ForgotPasswordCommandHandler
    : IRequestHandler<ForgotPasswordCommand, ForgotPasswordCommandResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(UserManager<AppUser> userManager, IEmailService emailService)
    {
        _userManager  = userManager;
        _emailService = emailService;
    }

    public async Task<ForgotPasswordCommandResponse> Handle(
    ForgotPasswordCommand request,
    CancellationToken cancellationToken)
{
    var user = await _userManager.FindByEmailAsync(request.Email);
    if (user == null)
        return new ForgotPasswordCommandResponse {
            Message = "Eğer bu e‑posta kayıtlıysa, şifre sıfırlama talimatları gönderilmiştir."
        };

    // 1) Token üret ve URL-safe hale getir
    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    var tokenBytes   = Encoding.UTF8.GetBytes(token);
    var urlSafeToken = WebEncoders.Base64UrlEncode(tokenBytes);

    // --- DEĞİŞİKLİK BURADA BAŞLIYOR ---

    // 2) Yeni, "path-based" deep link'i oluştur.
    // Artık '?' ve '&' kullanmıyoruz.
    var deepLink = $"tourbookingapp://reset-password" +
                   $"/{Uri.EscapeDataString(user.Email)}" + // '?' yerine '/'
                   $"/{urlSafeToken}";                     // '&' yerine '/'

    // 3) GitHub Pages redirect sayfası (bu kısım aynı kalabilir)
    var redirectBase = "https://ferenbulbul.github.io/tourbooking-redirect"; 
    // deep= parametresinin değeri artık yeni, sağlam linkimiz.
    var resetLink    = $"{redirectBase}?deep={deepLink}";

    // --- DEĞİŞİKLİK BURADA BİTİYOR ---

    // 4) E‑posta içeriği (HTML)
    var emailBody = $"""
        <h1>Şifre Sıfırlama Talebi</h1>
        <p>Merhaba {user.FirstName},</p>
        <p>Şifrenizi sıfırlamak için lütfen <a href="{resetLink}">buraya tıklayın</a>.</p>
        <p>E‑postanızı açıp bağlantıya tıkladığınızda mobil uygulaman açılacak ve şifre sıfırlama ekranına yönlendirileceksiniz.</p>
        <hr/>
        <p>Link çalışmazsa, şu adresi kopyalayıp tarayıcınıza yapıştırabilirsiniz:</p>
        <p>{resetLink}</p>
    """;

    await _emailService.SendEmailAsync(
        user.Email,
        "TourBooking – Şifre Sıfırlama Talebi",
        emailBody
    );

    return new ForgotPasswordCommandResponse {
        Message = "Eğer bu e‑posta kayıtlıysa, şifre sıfırlama talimatları gönderilmiştir."
    };
}
}
