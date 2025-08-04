using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features
{
    public class UpsertVehicleAvailableCommand : IRequest
    {
        public Guid? Id { get; set; }
        public Guid VehicleId { get; set; }
        public List<EventDto>? Events { get; set; }
    }
}
