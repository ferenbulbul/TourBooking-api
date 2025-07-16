using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Interfaces.Repositories
{

    public interface IEmailVerificationCodeRepository : IGenericRepository<EmailVerificationCode>
    {
        Task<EmailVerificationCode?> GetByUserIdAndCodeAsync(Guid userId, string code, CancellationToken cancellationToken);
    }

}