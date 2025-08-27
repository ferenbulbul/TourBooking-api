namespace TourBooking.Application.Features.Authentication.Commands.Register
{
    public class GoogleSignCommandResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserFullName { get; set; }
        public string Role { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsFirstLogin { get; set; }
        public bool IsProfileComplete { get; set; }
    }
}