using MediatR;

namespace TourBooking.Application.Features.Authentication.Commands.Register
{
    public class RegisterCommand : IRequest<RegisterCommandResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }

    }
}