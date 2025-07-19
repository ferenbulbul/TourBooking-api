using System;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Vehicle.Commands.AddVehicleType
{
    public class AddVehicleTypeCommand : IRequest
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
