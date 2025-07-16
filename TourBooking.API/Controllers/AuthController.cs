using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TourBooking.API.Controllers;
using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Features.Authentication.Commands.ForgotPassword;
using TourBooking.Application.Features.Authentication.Commands.Register;
using TourBooking.Application.Features.Authentication.Commands.ResetPassword;
using TourBooking.Application.Features.Authentication.Commands.SendEmailVerificationCode;
using TourBooking.Application.Features.Authentication.Commands.VerifyEmail;
using TourBooking.Application.Features.Queries.Login;
using TourBooking.Application.Interfaces.Services;

namespace TourBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery query)
        {
            LoginQueryResponse result = await _mediator.Send(query);
            return Ok(ApiResponse<LoginQueryResponse>.SuccessResponse(result, "Giriş başarılı."));
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<RegisterCommandResponse>), 201)]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            RegisterCommandResponse result = await _mediator.Send(command);
            return StatusCode(201, ApiResponse<RegisterCommandResponse>.SuccessResponse(result, result.Message, 201));
        }


        [HttpPost("send-verification-email")]
        [Authorize] 
        public async Task<IActionResult> SendVerificationEmail()
        {

            var userIdString = GetUserIdFromToken();

            var command = new SendEmailVerificationCodeCommand { UserId = userIdString };
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<SendEmailVerificationCodeCommandResponse>.SuccessResponse(null, result.Message));
        }

        [HttpPost("verify-email")]
        [Authorize] 
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequestDto request)
        {
            // Yine, işlem yapacak kullanıcının ID'sini token'dan alıyoruz.
            var userIdString = GetUserIdFromToken();
            
            var command = new VerifyEmailCommand
            {
                UserId = userIdString,
                Code = request.Code
            };
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<VerifyEmailCommandResponse>.SuccessResponse(result, result.Message));
        }
    }
    
}
