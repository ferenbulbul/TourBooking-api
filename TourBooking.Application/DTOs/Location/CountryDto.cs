using System;

namespace TourBooking.Application.DTOs
{
    public class CountryDto : BaseDto
    {
        public Guid? Id { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
