namespace TourBooking.Application.DTOs
{
    public class EventDto
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Start { get; set; }
        public bool AllDay { get; set; }
        public string? Calendar { get; set; }
    }
}
