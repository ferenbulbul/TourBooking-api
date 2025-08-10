using MediatR;

namespace TourBooking.Application.Features.Authentication.Commands.Register
{
    public class GuideRegisterCommand : IRequest<GuideRegisterCommandResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
