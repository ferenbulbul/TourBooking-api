using MediatR;
using Microsoft.AspNetCore.Identity;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
// Localization için kullandığın namespace'i ekle
using Microsoft.Extensions.Localization;
using TourBooking.Shared.Localization;
using TourBooking.Domain.Enums;


namespace TourBooking.Application.Features.Authentication.Commands.Register
{
    public class GoogleSignCommandHandler : IRequestHandler<GoogleSignCommand, GoogleSignCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public GoogleSignCommandHandler(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _localizer = localizer;
        }

        public async Task<GoogleSignCommandResponse> Handle(
            GoogleSignCommand request,
            CancellationToken cancellationToken)
        {
            const string loginProvider = "Firebase";
            var user = await _userManager.FindByLoginAsync(loginProvider, request.Uid);

            if (user is null)
            {
                // Kullanıcı harici giriş ile bulunamadı, şimdi email ile arayalım.
                user = await _userManager.FindByEmailAsync(request.Email);

                if (user is not null)
                {
                    // Email ile bir kullanıcı bulduk! Şimdi Google girişini bu hesaba bağlayalım.
                    Console.WriteLine($"User found by email '{request.Email}'. Linking Google account.");
                    var info = new UserLoginInfo(loginProvider, request.Uid, "Google");
                    var linkResult = await _userManager.AddLoginAsync(user, info);

                    if (!linkResult.Succeeded)
                    {
                        // Middleware'in yakalaması için uygun bir exception fırlat.
                        // 409 Conflict bu durum için uygundur.
                        throw new BusinessRuleValidationException(_localizer["FailedToLinkExternalAccount"]);
                    }
                }
                else
                {
                    // Bu kullanıcı sistemde hiç yok. Yeni bir AppUser oluşturalım.
                    Console.WriteLine($"Creating a new user for email '{request.Email}'.");
                    var newUser = new AppUser
                    {
                        Email = request.Email,
                        UserName = request.Email,
                        FirstName = request.Name, // veya FullName, modelinize göre
                        EmailConfirmed = true,
                        UserType=UserType.Customer
                    };

                    var createResult = await _userManager.CreateAsync(newUser);
                    if (!createResult.Succeeded)
                    {
                        // ValidationException, 400 Bad Request dönecektir, bu durum için uygun.
                        var errors = createResult.Errors.Select(e => e.Description).ToList();
                        throw new ValidationException(errors);
                    }

                    // Şimdi yeni oluşturulan kullanıcıya Google girişini bağlayalım.
                    var info = new UserLoginInfo(loginProvider, request.Uid, "Google");
                    var addLoginResult = await _userManager.AddLoginAsync(newUser, info);
                    if (!addLoginResult.Succeeded)
                    {
                        // Bu da bir iş kuralı ihlali sayılır.
                        throw new BusinessRuleValidationException(_localizer["FailedToAddLoginToNewUser"]);
                    }
                    user = newUser; // JWT oluşturmak için yeni kullanıcıyı ata
                }
            }

            // Bu noktaya geldiğimizde, 'user' nesnesinin dolu olduğundan eminiz.
            // Şimdi bu kullanıcı için kendi token'ımızı oluşturalım.
            var tokenDto = await _tokenService.CreateTokenAsync(user);
            user.RefreshToken = tokenDto.RefreshToken;
            user.RefreshTokenExpireDate = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new GoogleSignCommandResponse
            {
                Role = user.UserType.ToString(),
                AccessToken = tokenDto.AccessToken,
                RefreshToken = tokenDto.RefreshToken,
                EmailConfirmed = user.EmailConfirmed,
                UserFullName = user.FirstName,
                IsFirstLogin=false,
            };
        }
    }
}