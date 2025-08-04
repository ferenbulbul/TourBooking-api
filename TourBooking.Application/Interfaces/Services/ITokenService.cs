using TourBooking.Application.Common.Auth;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task<TokenResponse> CreateTokenAsync(AppUser user);
    }
}