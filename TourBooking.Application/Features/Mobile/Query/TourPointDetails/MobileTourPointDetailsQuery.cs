using System;
using MediatR;

namespace TourBooking.Application.Features.Mobile.Query.TourPointDetails
{
    public class MobileTourPointDetailsQuery : IRequest<MobileTourPointDetailsQueryResponse>
    {
        public Guid TourPointId { get; set; }
     }
}
