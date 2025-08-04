namespace TourBooking.Application.Common.Auth
{
    public record TokenResponse(string AccessToken, string RefreshToken);
}