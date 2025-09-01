namespace TourBooking.Application.DTOs
{
    public class AvailableDto
    {
        public Guid? Id { get; set; }
        public Guid VehicleId { get; set; }
        public List<EventDto>? Events { get; set; }
    }

    public record CalendarEventDto(string start, string calendar); // start: "YYYY-MM-DD"

    public record PostAvailabilityRequest(Guid vehicleId, List<CalendarEventDto> events);

    public record AvailabilityResponse(Guid id, Guid vehicleId, List<string> busyDays);

    public record PatchAvailabilityRequest(List<CalendarEventDto> events);

    public record DriverLocationDto(
        Guid DriverId,
        string NameSurname,
        string LicensePlate,
        double Latitude,
        double Longitude,
        string CompanyName
    );
    public record SystemCountDto(        
        int AgencyCount,
        int GuideCount,
        int VehicleCount,
        int TourPointCount,
        int BookingCount,
        int CustomerCount    
    );
}
