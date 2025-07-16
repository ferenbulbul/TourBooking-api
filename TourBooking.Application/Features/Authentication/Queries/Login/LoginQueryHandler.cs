using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginQueryResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public LoginQueryHandler(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<LoginQueryResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new AuthenticationFailedException("Kullanıcı adı veya şifre hatalı.");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                throw new AuthenticationFailedException("Kullanıcı adı veya şifre hatalı.");
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