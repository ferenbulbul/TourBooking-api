using System;
using MediatR;

namespace TourBooking.Application.Features
{
    public class MobileDetailedSearchQuery : IRequest<MobileDetailedSearchQueryResponse>
    {
        public int Type { get; set; }
        public Guid? RegionId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? DistrictId { get; set; }
    }
}
