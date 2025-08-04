namespace TourBooking.Application.DTOs
{
    public class DistrictDto : BaseDto
    {
        public Guid? Id { get; set; }
        public Guid CityId { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
