using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using TourBooking.Shared.Localization;

namespace TourBooking.Application.Features.Authentication.Commands.SendEmailVerificationCode
{
    public class SendPhoneNumberCodeCommandHandler : IRequestHandler<SendPhoneNumberCodeCommand>
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly INetgsmSmsService _smsService;
        private readonly IEmailVerificationCodeRepository _repository;
        IStringLocalizer<SharedResource> _localizer;

        public SendPhoneNumberCodeCommandHandler(
            UserManager<AppUser> userManager,
            IEmailService emailService,
            IEmailVerificationCodeRepository repository,
            IStringLocalizer<SharedResource> localizer
,
            INetgsmSmsService smsService)
        {
            _userManager = userManager;
            _repository = repository;
            _localizer = localizer;
            _smsService = smsService;
        }

        public async Task Handle(
            SendPhoneNumberCodeCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                throw new NotFoundException(_localizer["UserNotFound"]);
            }
            if (user.PhoneNumberConfirmed)
            {
                throw new BusinessRuleValidationException("Zaten Doğrulanmış");
            }

            var code = new Random().Next(100000, 999999).ToString("D6");
            var verificationCode = new EmailVerificationCode
            {
                UserId = user.Id,
                Code = code,
                ExpiryDate = DateTime.UtcNow.AddMinutes(3),
                IsUsed = false
            };

            await _repository.AddAsync(verificationCode);

            var body = $"{_localizer["EmailVerificationCodePrefix"]} <h2>{code}</h2>";
            var sms = new SmsMessageDto(user.PhoneNumber,body);
            List<SmsMessageDto> list = new();
            list.Add(sms);
            await _smsService.SendBatchAsync(list); 



        }
    }
}
