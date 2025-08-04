using MediatR;

namespace TourBooking.Application.Features;

public class VehicleQuery : IRequest<VehicleQueryResponse>
{
    public Guid AgencyId { get; set; }
}
