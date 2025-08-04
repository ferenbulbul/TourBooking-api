namespace TourBooking.Application.DTOs
{
    public class SeatTypeDto
    {
        public Guid Id { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
