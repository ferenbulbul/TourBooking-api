namespace TourBooking.Application.DTOs.Mobile
{
    public record NearbyTourPointDto
    (
        Guid Id,
        string CityName,
        string TourTypeName,
        string Title,
        string MainImage,
        double Distance
    );



}
