using System;
using MediatR;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class AddLanguageCommand : IRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
    }
}
