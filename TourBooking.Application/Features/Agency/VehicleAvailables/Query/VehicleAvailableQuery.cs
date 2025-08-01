using System;
using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features
{
    public class VehicleAvailableQuery : IRequest<VehicleAvailableQueryResponse>
    {
        public Guid VehicleId { get; set; }
    }
}
