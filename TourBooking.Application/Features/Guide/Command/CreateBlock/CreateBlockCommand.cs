using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings
{
    public class CreateBlockCommand : IRequest
    {
        public Guid GuideId { get; set; }
        public DateOnly Start { get; set; }
        public DateOnly End { get; set; }
        public string? Note { get; set; }
    }
}
