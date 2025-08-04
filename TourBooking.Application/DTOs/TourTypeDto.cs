namespace TourBooking.Application.DTOs
{
    public class TourTypeDto
    {
        public Guid Id { get; set; }
        public List<TranslationDto> Translations { get; set; }
        public string MainImageUrl { get; set; }
        public string ThumbImageUrl { get; set; }
    }
}