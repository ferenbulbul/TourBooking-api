namespace TourBooking.Application.DTOs
{
    public class VehicleClassDto : BaseDto
    {
        public Guid Id { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
