using FluentValidation;

namespace TourBooking.Application.Features.Authentication.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("İsim alanı zorunludur.")
                .MaximumLength(50).WithMessage("İsim en fazla 50 karakter olabilir.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyisim alanı zorunludur.")
                .MaximumLength(50).WithMessage("Soyisim en fazla 50 karakter olabilir.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta alanı zorunludur.")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Geçerli bir e-posta giriniz.");
                

           RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre alanı zorunludur.")
            .MinimumLength(9).WithMessage("Şifre en az 9 karakter olmalıdır.")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("Şifre en az bir özel karakter içermelidir.")
            .Matches(@"\d").WithMessage("Şifre en az bir rakam ('0'-'9') içermelidir.")
            .Matches(@"[A-Z]").WithMessage("Şifre en az bir büyük harf ('A'-'Z') içermelidir.");
        }
    }
}