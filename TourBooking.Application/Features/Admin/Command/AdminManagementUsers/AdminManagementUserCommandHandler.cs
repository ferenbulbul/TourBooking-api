using System;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TourBooking.Application.DTOs.Admin;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features.Admin
{
    public class AdminManagementUserCommandHandler
        : IRequestHandler<AdminManagementUserCommand>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public AdminManagementUserCommandHandler(UserManager<AppUser> manager, IEmailService emailService)
        {
            _userManager = manager;
            _emailService = emailService;
        }

        public async Task Handle(
            AdminManagementUserCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Id.HasValue)
            {
                // UPDATE
                var user = await _userManager.FindByIdAsync(request.Id.Value.ToString());
                if (user == null)
                    throw new BusinessRuleValidationException("Kullanıcı bulunamadı.");

                // Benzersizlik kontrolleri (yalnızca değişiyorsa)
                if (!string.IsNullOrWhiteSpace(request.Email) &&
                    !string.Equals(request.Email, user.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var emailExists = await _userManager.Users
                        .AnyAsync(u => u.Email != null &&
                                       u.Email.ToLower() == request.Email!.ToLower() &&
                                       u.Id != user.Id, cancellationToken);
                    if (emailExists)
                        throw new BusinessRuleValidationException("Bu e-posta başka bir kullanıcıda kayıtlı.");
                }

                if (!string.IsNullOrWhiteSpace(request.PhoneNumber) &&
                    !string.Equals(request.PhoneNumber, user.PhoneNumber, StringComparison.Ordinal))
                {
                    var phoneExists = await _userManager.Users
                        .AnyAsync(u => u.PhoneNumber == request.PhoneNumber && u.Id != user.Id, cancellationToken);
                    if (phoneExists)
                        throw new BusinessRuleValidationException("Bu telefon numarası başka bir kullanıcıda kayıtlı.");
                }

                // Patch (yalnızca dolu gelenleri yaz)
                if (!string.IsNullOrWhiteSpace(request.Name))
                    user.FirstName = request.Name!;
                if (!string.IsNullOrWhiteSpace(request.Surname))
                    user.LastName = request.Surname!;
                if (request.Email != null)        // null gönderdiyse null yazılmasını istiyorsun varsayımıyla
                    user.Email = request.Email;
                if (request.PhoneNumber != null)
                    user.PhoneNumber = request.PhoneNumber;
                
                

                // UserName stratejisi: e-posta değiştiyse UserName’i de eşitlemek isteyebilirsin
                if (!string.IsNullOrWhiteSpace(request.Email))
                    user.UserName = request.Email;

                var updateRes = await _userManager.UpdateAsync(user);
                if (!updateRes.Succeeded)
                    throw new BusinessRuleValidationException(string.Join("; ", updateRes.Errors.Select(e => e.Description)));
            }
            else
            {
                // CREATE
                // Benzersizlik (opsiyonel ama iyi pratik)
                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    var emailExists = await _userManager.Users
                        .AnyAsync(u => u.Email != null &&
                                       u.Email.ToLower() == request.Email!.ToLower(), cancellationToken);
                    if (emailExists)
                        throw new BusinessRuleValidationException("Bu e-posta zaten kayıtlı.");
                }

                if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                {
                    var phoneExists = await _userManager.Users
                        .AnyAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);
                    if (phoneExists)
                        throw new BusinessRuleValidationException("Bu telefon numarası zaten kayıtlı.");
                }

                var userName = request.Email
                               ?? request.PhoneNumber
                               ?? Guid.NewGuid().ToString("N");

                var newUser = new AppUser
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    FirstName = request.Name,
                    UserType = request.UserType,
                    LastName = request.Surname,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsFirstLogin = true
                };

                // Şifresiz oluşturmak istersen:
                // var createRes = await _userManager.CreateAsync(newUser);
                // Geçici şifre ile oluşturmak istersen:
                var tempPassword = "Temp.123";
                var createRes = await _userManager.CreateAsync(newUser, tempPassword);

                var emailTitle = "Hoş Geldiniz! İlk Giriş Bilgileriniz";

                var emailBody = @"
                                        Merhaba,

                                        Sistemimize kaydınız başarıyla oluşturuldu. 
                                        Rolünüz : " + newUser.UserType.ToString() + @"
                                        İlk girişiniz için tanımlanan geçici şifreniz aşağıdadır:

                                        <strong>Kullanıcı Adı (Email):</strong> " + newUser.Email + @"<br/>
                                        <strong>Geçici Şifre:</strong> " + tempPassword + @" <br/><br/>

                                        ℹ️ Güvenliğiniz için ilk girişinizde sizden şifrenizi değiştirmeniz istenecektir. 

                                        📲 Uygulamayı indirmek için aşağıdaki bağlantıyı kullanabilirsiniz:<br/>
                                        <a href='https://play.google.com/...' target='_blank'>Android için indirin</a><br/>
                                        <a href='https://apps.apple.com/...' target='_blank'>iOS için indirin</a><br/><br/>

                                        Teşekkürler,<br/>
                                        Tourrent.ai Ekibi
                                        ";

                if (!createRes.Succeeded)
                    throw new BusinessRuleValidationException(string.Join("; ", createRes.Errors.Select(e => e.Description)));
                await _emailService.SendEmailAsync(newUser.Email, emailTitle, emailBody);

            }
        }
    }
}
