using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public ForgotPasswordCommandHandler(UserManager<AppUser> userManager, IEmailService emailService, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _emailService = emailService;
            _unitOfWork = unitOfWork;

        }

        public async Task Handle(
        ForgotPasswordCommand request,
        CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) { throw new NotFoundException("Kullanıcı bulunamadı."); }
            var code = new Random().Next(100000, 999999).ToString("D6");
            var verificationCode = new EmailVerificationCode
            {
                UserId = user.Id,
                Code = code,
                ExpiryDate = DateTime.UtcNow.AddMinutes(3),
                IsUsed = false
            };
            await _unitOfWork.GetRepository<EmailVerificationCode>().AddAsync(verificationCode);
            var emailBody = $"Şifre Sıfırlama kodunuz: <h2>{code}</h2>";
            await _emailService.SendEmailAsync(user.Email!, "Şifre Sıfırlama  Kodu", emailBody);
        }

    }
}
