using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.Common.Auth
{
    public record TokenResponse(string AccessToken, string RefreshToken);
}