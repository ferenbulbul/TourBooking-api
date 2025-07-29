using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand,ResetPasswordCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailVerificationCodeRepository _repository;

        public ResetPasswordCommandHandler(UserManager<AppUser> userManager,IEmailVerificationCodeRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<ResetPasswordCommandResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user =await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new BusinessRuleValidationException("Doğrulama koduyla ilişkili kullanıcı bulunamadı.");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var identityResult= await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            
           ;

            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                throw new Expactions.ValidationException(errors);
            }
            return new ResetPasswordCommandResponse { };
        }
    }
}