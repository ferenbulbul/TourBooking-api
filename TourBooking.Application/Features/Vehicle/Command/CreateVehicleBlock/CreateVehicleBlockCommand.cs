using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings
{
    public class CreateVehicleBlockCommand : IRequest
    {
        public Guid VehicleId { get; set; }
        public DateOnly Start { get; set; }
        public DateOnly End { get; set; }
        public string? Note { get; set; }
    }
}
