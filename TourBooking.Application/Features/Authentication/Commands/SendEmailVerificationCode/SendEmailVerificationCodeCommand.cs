using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TourBooking.Application.Features.Authentication.Commands.SendEmailVerificationCode
{
    public class SendEmailVerificationCodeCommand : IRequest<SendEmailVerificationCodeCommandResponse>
    {
        public Guid UserId { get; set; }
    }
}