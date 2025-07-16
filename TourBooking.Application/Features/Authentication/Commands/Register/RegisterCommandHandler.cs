using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TourBooking.Application.Expactions;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegisterCommandResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // 1. İŞ KURALI KONTROLÜ: BU E-POSTA ZATEN VAR MI?
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                // HATA: İş kuralı ihlali. Exception fırlat.
                // Middleware bunu yakalayıp 409 Conflict'e çevirecek.
                throw new BusinessRuleValidationException("Bu e-posta adresi zaten kullanılmaktadır.");
            }

            // 2. YENİ KULLANICI NESNESİNİ OLUŞTUR
            var newUser = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                EmailConfirmed = false // Yeni kullanıcıların e-postasını onaylaması gerekir
            };

            // 3. KULLANICIYI VERİTABANINA KAYDET
            var result = await _userManager.CreateAsync(newUser, request.Password);

            // 4. IDENTITY'DEN GELEN SONUCU KONTROL ET
            if (!result.Succeeded)
            {
                // HATA: Identity bir hata döndürdü (örn: şifre politikası uyuşmazlığı).
                // Bu hataları alıp kendi ValidationException'ımıza koyuyoruz.
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new Expactions.ValidationException(errors);
            }
            

            // --- BAŞARILI AKIŞ ---

            // 5. BAŞARILI YANITI OLUŞTUR
            var response = new RegisterCommandResponse
            {
                UserId = newUser.Id,
                Message = "Kayıt işlemi başarılı. Lütfen e-postanızı kontrol ederek hesabınızı onaylayın."
            };

            return response;
        }
    }
}