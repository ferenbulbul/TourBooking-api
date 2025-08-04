namespace TourBooking.Application.DTOs
{
    public class VehicleBrandDto : BaseDto
    {
        public Guid Id { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
