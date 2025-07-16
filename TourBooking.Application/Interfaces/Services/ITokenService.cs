using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourBooking.Application.Common.Auth;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task<TokenResponse> CreateTokenAsync(AppUser user);
    }
}