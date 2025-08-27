using MediatR;

namespace TourBooking.Application.Features.Authentication.Commands.SendEmailVerificationCode
{
    public class SendPhoneNumberCodeCommand : IRequest
    {
        public Guid UserId { get; set; }
    }
}