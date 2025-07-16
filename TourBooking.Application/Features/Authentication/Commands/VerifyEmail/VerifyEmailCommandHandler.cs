using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Authentication.Commands.VerifyEmail
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, VerifyEmailCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailVerificationCodeRepository _repository;

        public VerifyEmailCommandHandler(UserManager<AppUser> userManager, IEmailVerificationCodeRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<VerifyEmailCommandResponse> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var verificationCode = await _repository.GetByUserIdAndCodeAsync(request.UserId, request.Code, cancellationToken);

            if (verificationCode == null)
            {
                throw new BusinessRuleValidationException("Geçersiz veya daha önce kullanılmış bir kod girdiniz.");
            }

            if (verificationCode.ExpiryDate < DateTime.UtcNow)
            {
                throw new BusinessRuleValidationException("Doğrulama kodunun süresi dolmuş. Lütfen yeni bir kod isteyin.");
            }

            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                throw new BusinessRuleValidationException("Doğrulama koduyla ilişkili kullanıcı bulunamadı.");
            }
            
            verificationCode.IsUsed = true;
            await _repository.UpdateAsync(verificationCode);

            user.EmailConfirmed = true;
            var identityResult = await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                throw new Expactions.ValidationException(errors);
            }

            return new VerifyEmailCommandResponse { Message = "E-posta adresiniz başarıyla doğrulandı." };
        }
    }
}