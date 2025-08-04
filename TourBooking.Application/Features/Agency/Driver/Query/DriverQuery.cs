using MediatR;

namespace TourBooking.Application.Features;

public class DriverQuery : IRequest<DriverQueryResponse>
{
    public Guid AgencyId { get; set; }
}
