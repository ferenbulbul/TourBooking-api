using System;
using MediatR;

namespace TourBooking.Application.Features
{
    public class LocationUpdateCommand : IRequest
    {
        public Guid UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
