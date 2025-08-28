using MediatR;

namespace TourBooking.Application.Features
{
    public class VerifyPhoneCommand : IRequest<VerifyPhoneCommandResponse>
    {
        public Guid UserId { get; set; } // Hangi kullanıcı
        public string Code { get; set; }   // Girdiği kod
    }
}