using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features
{
    public class UpsertTourPointCommand : IRequest
    {
        public Guid? Id { get; set; }
        public Guid CountryId { get; set; }
        public Guid RegionId { get; set; }
        public Guid CityId { get; set; }
        public Guid DistrictId { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid TourTypeId { get; set; }
        public string? MainImage { get; set; }
        public bool IsActive { get; set; }
        public bool IsHighlighted { get; set; }
        public List<string> OtherImages { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
