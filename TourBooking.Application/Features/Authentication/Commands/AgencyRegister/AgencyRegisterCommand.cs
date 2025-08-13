using MediatR;

namespace TourBooking.Application.Features.Authentication.Commands.Register
{
    public class AgencyRegisterCommand : IRequest<AgencyRegisterCommandResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FullAddress { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? CompanyName { get; set; }
        public string? PhoneNumber2 { get; set; }
        public string? PhoneNumber { get; set; }
        public string? TaxNumber { get; set; }
        public string? TursabUrl { get; set; }
    }
}
