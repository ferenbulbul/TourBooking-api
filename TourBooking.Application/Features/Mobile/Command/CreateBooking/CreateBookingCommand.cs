using System;
using MediatR;

namespace TourBooking.Application.Features
{
    public class CreateBookingCommand : IRequest<CreateBookingCommandResponse>
    {
        public Guid TourPointId { get; set; }
        public Guid? GuideId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid CityId { get; set; }
        public Guid DistrictId { get; set; }
        public Guid VehicleId { get; set; }
        public DateOnly Date { get; set; }
        public decimal TourPrice { get; set; }
        public decimal? GuidePrice { get; set; }
        public string? LocationDescription { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

    }
}
