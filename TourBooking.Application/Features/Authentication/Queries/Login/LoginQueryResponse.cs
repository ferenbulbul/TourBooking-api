namespace TourBooking.Application.Features.Queries.Login
{
    public class LoginQueryResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserFullName { get; set; }
        public string Role { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsFirstLogin { get; set; }
    }
}