using MediatR;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class FetchVehicleEventsQuery : IRequest<FetchVehicleEventsQueryResponse>
    {
        public Guid VehicleId { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
    }
}
