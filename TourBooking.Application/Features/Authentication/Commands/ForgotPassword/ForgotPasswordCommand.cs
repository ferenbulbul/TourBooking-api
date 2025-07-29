using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TourBooking.Application.Features
{
    public class ForgotPasswordCommand : IRequest
    {
        public string Email { get; set; }
    }
}