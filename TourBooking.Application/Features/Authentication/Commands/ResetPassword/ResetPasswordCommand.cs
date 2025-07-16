using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TourBooking.Application.Features.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<ResetPasswordCommandResponse>
    {
        public string Email { get; set; }
        public string Token { get; set; } 
        public string NewPassword { get; set; }
    }
}