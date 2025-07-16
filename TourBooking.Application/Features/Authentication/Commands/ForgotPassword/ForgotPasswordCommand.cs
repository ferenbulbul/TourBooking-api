using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TourBooking.Application.Features.Authentication.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<ForgotPasswordCommandResponse>
    {
        public string Email { get; set; }
    }
}