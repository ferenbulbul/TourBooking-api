using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class UpsertVehicleClassCommand : IRequest
    {
        public Guid? Id { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
