using System;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class AddVehicleTypeCommand : IRequest
    {
        public string Code { get; set; }

        public string Title { get; set; }
        public string LanguageCode { get; set; }
        public bool IsActive { get; set; }
    }
}
