using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings
{
    public class RemoveVehicleBlockCommand : IRequest
    {
        public Guid VehicleId { get; set; }
        public Guid BlockId { get; set; }
    }
}
