using MediatR;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class FetchEventsQuery : IRequest<FetchEventsQueryResponse>
    {
        public Guid GuidId { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
    }
}
