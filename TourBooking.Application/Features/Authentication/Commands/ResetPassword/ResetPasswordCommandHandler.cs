using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
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
                throw new BusinessRuleValidationException("Şifre sıfırlama işlemi başarısız oldu. Lütfen tekrar deneyin.");
            }
            var tokenBytes = WebEncoders.Base64UrlDecode(request.Token);
            var decodedToken = Encoding.UTF8.GetString(tokenBytes);

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);

            if (!result.Succeeded)
            {
                throw new ValidationException(new List<string> {
                    "Şifre sıfırlama işlemi başarısız oldu. Lütfen geçerli bir bağlantı kullandığınızdan emin olun."
                });
            }

            return new ResetPasswordCommandResponse { Message = "Şifreniz başarıyla güncellenmiştir." };
        }
    }
}