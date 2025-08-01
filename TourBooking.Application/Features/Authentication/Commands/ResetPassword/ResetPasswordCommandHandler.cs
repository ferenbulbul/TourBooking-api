using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Shared.Localization;

namespace TourBooking.Application.Features.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand,ResetPasswordCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailVerificationCodeRepository _repository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ResetPasswordCommandHandler(UserManager<AppUser> userManager, IEmailVerificationCodeRepository repository, IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<ResetPasswordCommandResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user =await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new BusinessRuleValidationException(_localizer["ResetCodeUserNotFound"]);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var identityResult= await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            
           ;

            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                throw new Expactions.ValidationException(errors);
            }
            return new ResetPasswordCommandResponse { };
        }
    }
}