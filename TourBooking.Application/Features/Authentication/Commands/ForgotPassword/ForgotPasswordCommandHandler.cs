using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Authentication.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(UserManager<AppUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<ForgotPasswordCommandResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new ForgotPasswordCommandResponse { Message = "Eğer bu e-posta adresi kayıtlıysa, şifre sıfırlama talimatları gönderilmiştir." };
            }

            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // ---- MOBİL UYGULAMA İÇİN LİNK OLUŞTURMA (EN KRİTİK KISIM) ----
            // Burada bir "deep link" veya "universal link" şeması kullanacağız.
            // Bu link, mobil uygulamayı açacak şekilde tasarlanmalıdır.
            var resetLink = $"tourbookingapp://reset-password?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(passwordResetToken)}";


            var emailBody = $"""
                <h1>Şifre Sıfırlama</h1>
                 <p>Merhaba {user.FirstName},</p>
               <p>Şifrenizi sıfırlamak için lütfen aşağıdaki linke tıklayın. Bu linkin mobil cihazınızda açılması gerekmektedir.</p>
                 <br>
               <a href="{resetLink}">{resetLink}</a> 
              <br>
                 <p>Eğer link çalışmazsa, yukarıdaki adresi kopyalayıp tarayıcınıza yapıştırabilirsiniz.</p>
             """;

            await _emailService.SendEmailAsync(user.Email, "TourBooking - Şifre Sıfırlama Talebi", emailBody);

            return new ForgotPasswordCommandResponse { Message = "Eğer bu e-posta adresi kayıtlıysa, şifre sıfırlama talimatları gönderilmiştir." };
        }
    }
}