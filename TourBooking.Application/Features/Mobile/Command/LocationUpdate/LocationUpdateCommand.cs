using System;
using MediatR;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features
{
    public class LocationUpdateCommand : IRequest
    {
        public Guid UserId { get; set; }
        public UserType? Role { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
