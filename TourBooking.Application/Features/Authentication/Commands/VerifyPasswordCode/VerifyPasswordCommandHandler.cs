using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Authentication.Commands.VerifyPasswordCode
{
    public class VerifyPasswordCommandHandler : IRequestHandler<VerifyPasswordCommand>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailVerificationCodeRepository _repository;

        public VerifyPasswordCommandHandler(UserManager<AppUser> userManager, IEmailVerificationCodeRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async Task Handle(VerifyPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new BusinessRuleValidationException("Doğrulama koduyla ilişkili kullanıcı bulunamadı.");
            }
            var verificationCode = await _repository.GetByUserIdAndCodeAsync(user.Id, request.Code, cancellationToken);
            if (verificationCode == null)
            {
                throw new BusinessRuleValidationException("Geçersiz veya daha önce kullanılmış bir kod girdiniz.");
            }

            if (verificationCode.ExpiryDate < DateTime.UtcNow)
            {
                throw new BusinessRuleValidationException("Doğrulama kodunun süresi dolmuş. Lütfen yeni bir kod isteyin.");
            }
            verificationCode.IsUsed = true;
            await _repository.UpdateAsync(verificationCode);
            
        }
    }
}