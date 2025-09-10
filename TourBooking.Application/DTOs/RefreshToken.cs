using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs
{
    public record RefreshTokenRequest(string RefreshToken);
    public record AuthResponse(string AccessToken, string RefreshToken);
}