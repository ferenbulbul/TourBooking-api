using System;
using MediatR;

namespace TourBooking.Application.Features
{
    public class TourBookingSummaryQuery : IRequest<TourBookingSummaryQueryResponse>
    {
        public Guid TourPointId { get; set; }
        public Guid GuideId { get; set; }
        public Guid DistrictId { get; set; }
        public Guid VehicleId { get; set; } 
        public DateOnly? Date { get; set; }           
    }
}
