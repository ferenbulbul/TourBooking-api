using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Infrastructure.Context;

namespace TourBooking.Infrastructure.Repositories
{
    public class EmailVerificationCodeRepository
        : GenericRepository<EmailVerificationCode>,
            IEmailVerificationCodeRepository
    {
        public EmailVerificationCodeRepository(AppDbContext context)
            : base(context) { }

        public async Task<EmailVerificationCode?> GetByUserIdAndCodeAsync(
            Guid userId,
            string code,
            CancellationToken cancellationToken
        )
        {
            return await _context.EmailVerificationCodes.FirstOrDefaultAsync(
                vc => vc.UserId == userId && vc.Code == code && !vc.IsUsed && !vc.IsDeleted,
                cancellationToken
            );
        }
    }
}
