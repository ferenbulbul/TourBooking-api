using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TourBooking.Application.Features.Authentication.Commands.Register
{
    public class RegisterCommand : IRequest<RegisterCommandResponse>
    {
        // Controller'a JSON body olarak gönderilecek olan alanlar.
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}