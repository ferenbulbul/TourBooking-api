using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using TourBooking.Application.Expactions;
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

        public LoginQueryHandler(UserManager<AppUser> userManager, ITokenService tokenService, IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _localizer = localizer;

        }

        public async Task<LoginQueryResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new AuthenticationFailedException(_localizer["InvalidLogin"]);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                throw new AuthenticationFailedException(_localizer["InvalidLogin"]);
            }

            var tokens = await _tokenService.CreateTokenAsync(user);
            var response = new LoginQueryResponse
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                UserFullName = $"{user.FirstName} {user.LastName}",
                Role = user.UserType.ToString(),
                EmailConfirmed = user.EmailConfirmed
            };
            return response;
        }
    }
}