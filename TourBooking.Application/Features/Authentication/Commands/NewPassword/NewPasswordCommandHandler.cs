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

        public async Task Handle(NewPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new BusinessRuleValidationException(_localizer["ResetCodeUserNotFound"]);
            }
            await _userManager.ChangePasswordAsync(user, user.PasswordHash, request.Password);
        }
    }
}