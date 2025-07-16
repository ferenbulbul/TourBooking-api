using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Authentication.Commands.SendEmailVerificationCode
{
    public class SendEmailVerificationCodeCommandHandler : IRequestHandler<SendEmailVerificationCodeCommand, SendEmailVerificationCodeCommandResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IEmailVerificationCodeRepository _repository;

    public SendEmailVerificationCodeCommandHandler(UserManager<AppUser> userManager, IEmailService emailService, IEmailVerificationCodeRepository repository)
    {
        _userManager = userManager;
        _emailService = emailService;
        _repository = repository;
    }

    public async Task<SendEmailVerificationCodeCommandResponse> Handle(SendEmailVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) { throw new NotFoundException("Kullanıcı bulunamadı."); }
        if (user.EmailConfirmed) { throw new BusinessRuleValidationException("Bu e-posta adresi zaten doğrulanmış."); }
        
        var code = new Random().Next(100000, 999999).ToString("D6");
        var verificationCode = new EmailVerificationCode
        {
            UserId = user.Id,
            Code = code,
            ExpiryDate = DateTime.UtcNow.AddMinutes(3),
            IsUsed = false
        };

        await _repository.AddAsync(verificationCode);
        var emailBody = $"E-posta doğrulama kodunuz: <h2>{code}</h2>";
        await _emailService.SendEmailAsync(user.Email, "E-posta Doğrulama Kodu", emailBody);

        return new SendEmailVerificationCodeCommandResponse { Message = $"'{user.Email}' adresine doğrulama kodu gönderildi." };
    }
}
}