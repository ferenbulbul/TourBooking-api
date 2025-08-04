using MediatR;

namespace TourBooking.Application.Features
{
    public class VehicleAvailableQuery : IRequest<VehicleAvailableQueryResponse>
    {
        public Guid VehicleId { get; set; }
    }
}
