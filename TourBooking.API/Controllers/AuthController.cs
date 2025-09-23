using System.Text.Json;
using FirebaseAdmin.Auth;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Crypto;
using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Features;
using TourBooking.Application.Features.Authentication.Commands;
using TourBooking.Application.Features.Authentication.Commands.Register;
using TourBooking.Application.Features.Authentication.Commands.ResetPassword;
using TourBooking.Application.Features.Authentication.Commands.SendEmailVerificationCode;
using TourBooking.Application.Features.Authentication.Commands.VerifyEmail;
using TourBooking.Application.Features.Authentication.Commands.VerifyPasswordCode;
using TourBooking.Application.Features.Queries.Login;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using TourBooking.Shared.Localization;

namespace TourBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;


        public AuthController(IMediator mediator, IStringLocalizer<SharedResource> localizer, UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _mediator = mediator;
            _localizer = localizer;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery query)
        {
            LoginQueryResponse result = await _mediator.Send(query);
            return Ok(
                ApiResponse<LoginQueryResponse>.SuccessResponse(result, _localizer["LoginSuccess"])
            );
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<RegisterCommandResponse>), 201)]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            RegisterCommandResponse result = await _mediator.Send(command);
            return StatusCode(
                201,
                ApiResponse<RegisterCommandResponse>.SuccessResponse(
                    result,
                    _localizer["RegistrationSuccessVerifyEmail"],
                    201
                )
            );
        }

        [HttpPost("agency-register")]
        [ProducesResponseType(typeof(ApiResponse<RegisterCommandResponse>), 201)]
        public async Task<IActionResult> AgencyRegister([FromBody] AgencyRegisterCommand command)
        {
            AgencyRegisterCommandResponse result = await _mediator.Send(command);
            return StatusCode(
                201,
                ApiResponse<AgencyRegisterCommandResponse>.SuccessResponse(
                    result,
                    _localizer["RegistrationSuccessVerifyEmail"],
                    201
                )
            );
        }

        [HttpPost("guide-register")]
        [ProducesResponseType(typeof(ApiResponse<GuideRegisterCommandResponse>), 201)]
        public async Task<IActionResult> GuideRegister([FromBody] GuideRegisterCommand command)
        {
            GuideRegisterCommandResponse result = await _mediator.Send(command);
            return StatusCode(
                201,
                ApiResponse<GuideRegisterCommandResponse>.SuccessResponse(
                    result,
                    _localizer["RegistrationSuccessVerifyEmail"],
                    201
                )
            );
        }

        [HttpPost("send-verification-email")]
        [Authorize]
        public async Task<IActionResult> SendVerificationEmail()
        {
            var userIdString = GetUserIdFromToken();

            var command = new SendEmailVerificationCodeCommand { UserId = userIdString };
            var result = await _mediator.Send(command);
            return Ok(
                ApiResponse<SendEmailVerificationCodeCommandResponse>.SuccessResponse(
                    null,
                    result.Message
                )
            );
        }

        [HttpPost("verify-email")]
        [Authorize]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequestDto request)
        {
            var userIdString = GetUserIdFromToken();

            var command = new VerifyEmailCommand { UserId = userIdString, Code = request.Code };
            var result = await _mediator.Send(command);

            return Ok(
                ApiResponse<VerifyEmailCommandResponse>.SuccessResponse(
                    result,
                    _localizer["EmailVerifiedSuccessfully"]
                )
            );
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.SuccessResponse(null, _localizer["CodeSentToEmail"]));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(
                ApiResponse<object>.SuccessResponse(null, _localizer["PasswordChangedSuccessfully"])
            );
        }

        [HttpPost("verify-password-code")]
        public async Task<IActionResult> ResetPassword([FromBody] VerifyPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(
                ApiResponse<object>.SuccessResponse(null, _localizer["VerificationCodeValid"])
            );
        }

        [HttpPost("signin-with-google")]
        public async Task<IActionResult> SignInWithGoogle([FromBody] FirebaseTokenRequest request)
        {


            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Token cannot be empty.");
            }

            try
            {

                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(request.Token);
                string uid = decodedToken.Uid;

                // 🔹 sign_in_provider'ı parse et
                string signInProvider = "unknown";
                if (decodedToken.Claims.TryGetValue("firebase", out object firebaseObj) && firebaseObj is JsonElement firebaseElement)
                {
                    if (firebaseElement.TryGetProperty("sign_in_provider", out var providerProp))
                    {
                        signInProvider = providerProp.GetString() ?? "unknown";
                    }
                }

                Console.WriteLine($"🔑 Provider: {signInProvider}");

                string email = (string)decodedToken.Claims.GetValueOrDefault("email", "N/A@gmail.com");
                string name = (string)decodedToken.Claims.GetValueOrDefault("name", "N/A");

                var response = await _mediator.Send(new GoogleSignCommand { Uid = uid, Email = email, Name = name });
                Console.WriteLine($"✅ Token Validated. UID: {uid}, Email: {email}, Name: {name}");


                return Ok(ApiResponse<GoogleSignCommandResponse>.SuccessResponse(response, "google giriş başarılı"));

                ;
            }
            catch (FirebaseAuthException ex)
            {
                // Token geçersiz, süresi dolmuş veya başka bir Firebase hatası var.
                return Unauthorized(new { Message = "Authentication failed. Invalid token.", Details = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("new-password")]
        public async Task<IActionResult> NewPassword([FromBody] NewPasswordDto request)
        {
            var userId = GetUserIdFromToken();

            await _mediator.Send(new NewPasswordCommand { UserId = userId.ToString(), Password = request.Password });
            return Ok(
                ApiResponse<object>.SuccessResponse(null, null)
            );
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest req)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == req.RefreshToken);

            if (user is null)
                return Unauthorized(ApiResponse<AuthResponse>.FailResponse("User not found"));


            if (user.RefreshToken != req.RefreshToken)
                return Unauthorized(ApiResponse<AuthResponse>.FailResponse("Invalid refresh token"));


            if (user.RefreshTokenExpireDate <= DateTime.UtcNow)
                return Unauthorized(ApiResponse<AuthResponse>.FailResponse("Refresh token expired"));


            try
            {
                var newToken = await _tokenService.CreateTokenAsync(user);
                user.RefreshToken = newToken.RefreshToken;
                user.RefreshTokenExpireDate = DateTime.UtcNow.AddDays(90);

                try
                {
                    await _userManager.UpdateAsync(user);
                }
                catch (Exception ex)
                {
                    // DB update sırasında hata
                    // _logger.LogError(ex, "UpdateAsync failed for user {UserId}", user.Id);
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        ApiResponse<AuthResponse>.FailResponse("User update failed"));
                }

                try
                {
                    var response = new AuthResponse(newToken.AccessToken, newToken.RefreshToken);
                    return Ok(ApiResponse<AuthResponse>.SuccessResponse(response, ""));
                }
                catch (Exception ex)
                {
                    // Response oluşturma sırasında hata
                    // _logger.LogError(ex, "AuthResponse creation failed for user {UserId}", user.Id);
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        ApiResponse<AuthResponse>.FailResponse("Response creation failed"));
                }
            }
            catch (Exception ex)
            {
                // Token üretiminde hata
                // _logger.LogError(ex, "CreateTokenAsync failed for user {UserId}", user.Id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<AuthResponse>.FailResponse("Token creation failed"));
            }
        }

    }
}
