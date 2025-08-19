using System;
using MediatR;

namespace TourBooking.Application.Features
{
    public class MobileTourPointByTourTypeQuery : IRequest<MobileTourPointByTourTypeQueryResponse>
    {
        public Guid TourType { get; set; }
      
    }
}
