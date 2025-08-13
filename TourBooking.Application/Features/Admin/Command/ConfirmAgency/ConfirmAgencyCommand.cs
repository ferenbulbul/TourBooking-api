using System;
using MediatR;

namespace TourBooking.Application.Features.Admin.Command.ConfirmAgency
{
    public class ConfirmAgencyCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
