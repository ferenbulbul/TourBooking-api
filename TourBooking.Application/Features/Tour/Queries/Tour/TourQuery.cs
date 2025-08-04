using MediatR;

namespace TourBooking.Application.Features;

public class TourQuery : IRequest<TourQueryResponse>
{
    public Guid AgencyId { get; set; }
}
