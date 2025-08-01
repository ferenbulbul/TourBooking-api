using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using TourBooking.Shared.Localization;

namespace TourBooking.Application.Features
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ForgotPasswordCommandHandler(UserManager<AppUser> userManager, IEmailService emailService, IUnitOfWork unitOfWork,IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task Handle(
        ForgotPasswordCommand request,
        CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) { throw new NotFoundException(_localizer["UserNotFound"]); }
            var code = new Random().Next(100000, 999999).ToString("D6");
            var verificationCode = new EmailVerificationCode
            {
                UserId = user.Id,
                Code = code,
                ExpiryDate = DateTime.UtcNow.AddMinutes(3),
                IsUsed = false
            };
            

            await _unitOfWork.GetRepository<EmailVerificationCode>().AddAsync(verificationCode);
            var emailBody = $"{_localizer["ResetPasswordCodePrefix"]} <h2>{code}</h2>";
            await _emailService.SendEmailAsync(user.Email!,$"{_localizer["ResetPasswordCodeTitle"]} ", emailBody);
        }
            
    }
}
