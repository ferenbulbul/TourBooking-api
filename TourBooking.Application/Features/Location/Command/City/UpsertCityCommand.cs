using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class UpsertCityCommand : IRequest
    {
        public Guid? Id { get; set; }
        public Guid RegionId { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
