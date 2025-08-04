using MediatR;

namespace TourBooking.Application.Features.Queries.Login
{
    public class LoginQuery : IRequest<LoginQueryResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}