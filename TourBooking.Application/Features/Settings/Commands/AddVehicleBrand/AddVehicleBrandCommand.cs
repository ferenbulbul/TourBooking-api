using System;
using MediatR;

namespace TourBooking.Application.Features.Vehicle.Commands.AddVehicleBrand
{
    public class AddVehicleBrandCommand : IRequest
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
