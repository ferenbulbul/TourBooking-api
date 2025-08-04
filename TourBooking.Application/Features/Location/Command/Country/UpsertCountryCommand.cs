using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class UpsertCountryCommand : IRequest
    {
         public Guid? Id { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
