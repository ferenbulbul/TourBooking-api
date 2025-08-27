using System;
using MediatR;

namespace TourBooking.Application.Features
{
    public class EditPhoneCommand : IRequest
    {
        public Guid UserId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
