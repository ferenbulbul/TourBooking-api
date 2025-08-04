namespace TourBooking.Application.DTOs
{
    public class RegionDto : BaseDto
    {
        public string CountryName { get; set; }
        public Guid? Id { get; set; }
        public Guid CountryId { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
