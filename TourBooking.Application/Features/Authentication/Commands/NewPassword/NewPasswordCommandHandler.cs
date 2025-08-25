using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Shared.Localization;

namespace TourBooking.Application.Features.Authentication.Commands
{
    public class NewPasswordCommandHandler : IRequestHandler<NewPasswordCommand>
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly IStringLocalizer<SharedResource> _localizer;

        public NewPasswordCommandHandler(UserManager<AppUser> userManager, IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;

            _localizer = localizer;
        }

        public async Task Handle(NewPasswordCommand request, CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
                throw new BusinessRuleValidationException(_localizer["ResetCodeUserNotFound"]);

            // Sunucuda token üret, e-posta göndermeden direkt kullan
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
            if (!result.Succeeded)
                throw new BusinessRuleValidationException(string.Join("; ", result.Errors.Select(e => e.Description)));

            user.IsFirstLogin = false;
            await _userManager.UpdateAsync(user);

        }
    }
}
