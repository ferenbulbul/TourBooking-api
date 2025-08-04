using MediatR;

namespace TourBooking.Application.Features
{
    public class ForgotPasswordCommand : IRequest
    {
        public string Email { get; set; }
    }
}