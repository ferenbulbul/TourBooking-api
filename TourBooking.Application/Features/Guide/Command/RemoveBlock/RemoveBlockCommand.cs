using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings
{
    public class RemoveBlockCommand : IRequest
    {
        public Guid GuideId { get; set; }
        public Guid BlockId { get; set; }
    }
}
