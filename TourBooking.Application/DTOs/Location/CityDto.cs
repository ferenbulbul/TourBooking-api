using System;

namespace TourBooking.Application.DTOs
{
    public class CityDto : BaseDto
    {
        public Guid? Id { get; set; }
        public Guid RegionId { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
