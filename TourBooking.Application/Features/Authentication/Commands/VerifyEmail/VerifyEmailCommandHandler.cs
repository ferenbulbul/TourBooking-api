using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Shared.Localization;

namespace TourBooking.Application.Features.Authentication.Commands.VerifyEmail
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, VerifyEmailCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailVerificationCodeRepository _repository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public VerifyEmailCommandHandler(UserManager<AppUser> userManager, IEmailVerificationCodeRepository repository, IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<VerifyEmailCommandResponse> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var verificationCode = await _repository.GetByUserIdAndCodeAsync(request.UserId, request.Code, cancellationToken);

            if (verificationCode == null)
            {
                throw new BusinessRuleValidationException(_localizer["InvalidOrUsedCode"]);
            }

            if (verificationCode.ExpiryDate < DateTime.UtcNow)
            {
                throw new BusinessRuleValidationException(_localizer["VerificationCodeExpired"]);
            }

            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                throw new BusinessRuleValidationException(_localizer["ResetCodeUserNotFound"]);
            }
            
            verificationCode.IsUsed = true;
            await _repository.UpdateAsync(verificationCode);

            user.EmailConfirmed = true;
            var identityResult = await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                throw new Expactions.ValidationException(errors);
            }

            return new VerifyEmailCommandResponse { Message = "" };
        }
    }
}