using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features
{
    public class UpsertTourCommand : IRequest
    {
        public Guid? Id { get; set; }
        public Guid TourPointId { get; set; }
        public List<PricingDto> Pricing { get; set; }
    }
}
