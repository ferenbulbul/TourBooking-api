using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using TourBooking.Shared.Localization;

namespace TourBooking.Application.Features.Authentication.Commands.SendEmailVerificationCode
{
    public class SendEmailVerificationCodeCommandHandler
        : IRequestHandler<
            SendEmailVerificationCodeCommand,
            SendEmailVerificationCodeCommandResponse
        >
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IEmailVerificationCodeRepository _repository;
        IStringLocalizer<SharedResource> _localizer;

        public SendEmailVerificationCodeCommandHandler(
            UserManager<AppUser> userManager,
            IEmailService emailService,
            IEmailVerificationCodeRepository repository,
            IStringLocalizer<SharedResource> localizer
        )
        {
            _userManager = userManager;
            _emailService = emailService;
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<SendEmailVerificationCodeCommandResponse> Handle(
            SendEmailVerificationCodeCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                throw new NotFoundException(_localizer["UserNotFound"]);
            }
            if (user.EmailConfirmed)
            {
                throw new BusinessRuleValidationException(_localizer["EmailAlreadyVerified"]);
            }

            var code = new Random().Next(100000, 999999).ToString("D6");
            var verificationCode = new EmailVerificationCode
            {
                UserId = user.Id,
                Code = code,
                ExpiryDate = DateTime.UtcNow.AddMinutes(3),
                IsUsed = false
            };

            await _repository.AddAsync(verificationCode);
            var body = $"{_localizer["EmailVerificationCodePrefix"]} <h2>{code}</h2>";
            await _emailService.SendEmailAsync(
                user.Email!,
                $"{_localizer["EmailVerificationCodeTitle"]}",
                body
            );
            var message = string.Format(_localizer["VerificationCodeSentTo"], user.Email);

            return new SendEmailVerificationCodeCommandResponse { Message = message };
        }
    }
}
