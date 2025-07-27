using System;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class AddVehicleTypeCommand : IRequest
    {
        public Guid? Id { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
