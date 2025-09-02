using MediatR;

namespace TourBooking.Application.Features
{
    public class ToggleFavoriteCommand : IRequest
    {
        public Guid CustomerId { get; set; }
        public Guid TourPointId { get; set; }
    }
}