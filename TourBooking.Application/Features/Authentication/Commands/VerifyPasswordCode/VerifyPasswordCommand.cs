using MediatR;

namespace TourBooking.Application.Features.Authentication.Commands.VerifyPasswordCode
{
    public class VerifyPasswordCommand:IRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}