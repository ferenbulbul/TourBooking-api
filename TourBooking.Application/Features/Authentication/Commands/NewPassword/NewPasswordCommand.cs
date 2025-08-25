using MediatR;

namespace TourBooking.Application.Features.Authentication.Commands
{
    public class NewPasswordCommand : IRequest
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}