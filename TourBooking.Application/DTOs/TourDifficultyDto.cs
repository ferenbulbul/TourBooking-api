using System;

namespace TourBooking.Application.DTOs
{
    public class TourDifficultyDto
    {
        public Guid Id { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
