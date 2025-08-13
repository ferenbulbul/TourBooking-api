using System;
using MediatR;

namespace TourBooking.Application.Features.Admin.Command.ConfirmGuide
{
    public class ConfirmGuideCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
