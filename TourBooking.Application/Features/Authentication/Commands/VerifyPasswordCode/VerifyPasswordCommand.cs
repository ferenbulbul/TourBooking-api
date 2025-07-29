using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TourBooking.Application.Features.Authentication.Commands.VerifyPasswordCode
{
    public class VerifyPasswordCommand:IRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}