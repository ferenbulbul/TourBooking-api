using MediatR;

namespace TourBooking.Application.Features;

public class NearbyTourPointsQuery : IRequest<NearbyTourPointsQueryResponse>
{
    public Guid CustomerId { get; set; }
 }
