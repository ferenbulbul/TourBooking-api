using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace TourBooking.API
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected Guid GetUserIdFromToken()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(idClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
            }
            return userId;
        }
    }
}