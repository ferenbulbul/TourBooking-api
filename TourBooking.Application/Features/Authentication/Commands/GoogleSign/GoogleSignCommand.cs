using MediatR;

namespace TourBooking.Application.Features.Authentication.Commands.Register
{
    public class GoogleSignCommand : IRequest<GoogleSignCommandResponse>
    {
        public string Uid { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}