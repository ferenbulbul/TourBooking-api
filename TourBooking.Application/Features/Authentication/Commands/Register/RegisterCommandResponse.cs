using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.Features.Authentication.Commands.Register
{
    public class RegisterCommandResponse
    {
        public Guid UserId { get; set; }
        public string Message { get; set; }
    }
}