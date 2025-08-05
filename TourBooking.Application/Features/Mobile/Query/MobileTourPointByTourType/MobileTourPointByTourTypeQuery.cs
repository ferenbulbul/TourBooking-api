using MediatR;

namespace TourBooking.Application.Features;

public class MobileTourPointByTourTypeQuery : IRequest<MobileTourPointByTourTypeQueryResponse>
{
    public Guid CagetoryId { get; set; }
 }
