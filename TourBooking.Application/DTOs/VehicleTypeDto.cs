namespace TourBooking.Application.DTOs
{
    public class VehicleTypeDto : BaseDto
    {
        public Guid Id { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
