using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Shared.Localization;

namespace TourBooking.Application.Features.Authentication.Commands.VerifyPasswordCode
{
    public class VerifyPasswordCommandHandler : IRequestHandler<VerifyPasswordCommand>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailVerificationCodeRepository _repository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public VerifyPasswordCommandHandler(UserManager<AppUser> userManager, IEmailVerificationCodeRepository repository, IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _repository = repository;
            _localizer = localizer;
        }

        public async Task Handle(VerifyPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new BusinessRuleValidationException(_localizer["ResetCodeUserNotFound"]);
            }
            var verificationCode = await _repository.GetByUserIdAndCodeAsync(user.Id, request.Code, cancellationToken);
            if (verificationCode == null)
            {
                throw new BusinessRuleValidationException(_localizer["InvalidOrUsedCode"]);
            }

            if (verificationCode.ExpiryDate < DateTime.UtcNow)
            {
                throw new BusinessRuleValidationException(_localizer["VerificationCodeExpired"]);
            }
            verificationCode.IsUsed = true;
            await _repository.UpdateAsync(verificationCode);
            
        }
    }
}