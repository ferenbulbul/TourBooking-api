using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TourBooking.Application.Features.Authentication.Commands.VerifyEmail
{
    public class VerifyEmailCommand : IRequest<VerifyEmailCommandResponse>
    {
        public Guid UserId { get; set; } // Hangi kullanıcı
        public string Code { get; set; }   // Girdiği kod
    }
}