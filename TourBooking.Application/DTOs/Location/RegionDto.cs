using System;

namespace TourBooking.Application.DTOs
{
    public class RegionDto : BaseDto
    {
        public Guid? Id { get; set; }
        public Guid CountryId { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
