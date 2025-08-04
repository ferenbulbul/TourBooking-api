namespace TourBooking.Application.DTOs
{
    public class BaseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
