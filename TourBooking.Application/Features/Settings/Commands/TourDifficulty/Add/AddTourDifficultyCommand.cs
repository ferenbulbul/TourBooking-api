using System;
using MediatR;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class AddTourDifficultyCommand : IRequest
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
