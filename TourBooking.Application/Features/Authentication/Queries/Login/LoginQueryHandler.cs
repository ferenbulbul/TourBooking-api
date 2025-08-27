using System;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using TourBooking.Application.Expactions;
using TourBooking.Application.Features.Authentication.Queries.IsApproved;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using TourBooking.Shared.Localization;

namespace TourBooking.Application.Features.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginQueryResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMediator _mediator;

        public LoginQueryHandler(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            IStringLocalizer<SharedResource> localizer,
            IMediator mediator
        )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _localizer = localizer;
            _mediator = mediator;
        }

        public async Task<LoginQueryResponse> Handle(
            LoginQuery request,
            CancellationToken cancellationToken
        )
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException(_localizer["InvalidLogin"]);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                throw new NotFoundException(_localizer["InvalidLogin"]);
            }

            var isConfirmed = await _mediator.Send(
                new IsApprovedQuery { Role = user.UserType.ToString(), UserId = user.Id }
            );

            if (!isConfirmed.IsApproved)
            {
                throw new NotConfimedException("Kullanıcınız henüz onaylanmadı");
            }

            var tokens = await _tokenService.CreateTokenAsync(user);
            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenExpireDate = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);
            var response = new LoginQueryResponse
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                UserFullName = $"{user.FirstName} {user.LastName}",
                Role = user.UserType.ToString(),
                EmailConfirmed = user.EmailConfirmed,
                IsFirstLogin = user.IsFirstLogin ?? false,
                IsProfileComplete = user.PhoneNumberConfirmed
            };
            return response;
        }
    }
}
