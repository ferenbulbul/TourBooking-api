using System;
using MediatR;

namespace TourBooking.Application.Features.Vehicle.Commands.AddVehicleBrand
{
    public class UpdateVehicleBrandCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
