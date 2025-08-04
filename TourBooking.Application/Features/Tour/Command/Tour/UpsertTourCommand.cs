using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features
{
    public class UpsertTourCommand : IRequest
    {
        public Guid AgencyId { get; set; }
        public Guid? Id { get; set; }
        public Guid TourPointId { get; set; }
        public List<PricingDto> Pricing { get; set; }
    }
}
