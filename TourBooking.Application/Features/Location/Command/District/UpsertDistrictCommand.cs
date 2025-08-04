using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features
{
    public class UpsertDistrictCommand : IRequest
    {
        public Guid? Id { get; set; }
        public Guid CityId { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
