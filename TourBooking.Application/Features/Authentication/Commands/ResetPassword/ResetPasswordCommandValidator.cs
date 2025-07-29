using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace TourBooking.Application.Features.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta alanı zorunludur.")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Geçerli bir e-posta giriniz.");
            RuleFor(x => x.NewPassword)
             .NotEmpty().WithMessage("Şifre alanı zorunludur.")
             .MinimumLength(9).WithMessage("Şifre en az 9 karakter olmalıdır.")
             .Matches(@"[^a-zA-Z0-9]").WithMessage("Şifre en az bir özel karakter içermelidir.")
             .Matches(@"\d").WithMessage("Şifre en az bir rakam ('0'-'9') içermelidir.")
             .Matches(@"[A-Z]").WithMessage("Şifre en az bir büyük harf ('A'-'Z') içermelidir.");
        }
    }
}