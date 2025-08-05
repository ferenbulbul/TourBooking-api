using MediatR;

namespace TourBooking.Application.Features;

public class MobileTourPointBySearchQuery : IRequest<MobileTourPointBySearchQueryResponse>
{
    public string Query { get; set; }
}
