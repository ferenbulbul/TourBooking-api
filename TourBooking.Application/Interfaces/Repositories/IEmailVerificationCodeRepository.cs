using TourBooking.Domain.Entities;

namespace TourBooking.Application.Interfaces.Repositories
{

    public interface IEmailVerificationCodeRepository : IGenericRepository<EmailVerificationCode>
    {
        Task<EmailVerificationCode?> GetByUserIdAndCodeAsync(Guid userId, string code, CancellationToken cancellationToken);
    }

}