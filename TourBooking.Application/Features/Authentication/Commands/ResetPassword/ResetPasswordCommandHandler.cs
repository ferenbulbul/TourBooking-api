using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TourBooking.Application.Expactions;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;

        public ResetPasswordCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResetPasswordCommandResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Hata mesajlarını genel tutmak güvenlik için önemlidir.
                throw new BusinessRuleValidationException("Şifre sıfırlama işlemi başarısız oldu. Lütfen tekrar deneyin.");
            }

            // Identity'nin ResetPasswordAsync metodu hem token'ı doğrular hem de şifreyi günceller.
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (!result.Succeeded)
            {
                // Token geçersizse veya yeni şifre politikalara uymuyorsa burası çalışır.
                // Identity'nin hatalarını alıp kendi exception'ımıza koyabiliriz.
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException(errors);
            }

            return new ResetPasswordCommandResponse { Message = "Şifreniz başarıyla güncellenmiştir." };
        }
    }
}