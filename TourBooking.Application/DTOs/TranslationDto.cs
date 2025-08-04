namespace TourBooking.Application.DTOs
{
    public class TranslationDto
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public Guid LanguageId { get; set; }
        public string LanguageName { get; set; }
    }
}
