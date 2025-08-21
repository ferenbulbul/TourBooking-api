// TourBooking.Application/Features/LocationUpdate/LocationUpdateCommandHandler.cs
using MediatR;
using Microsoft.AspNetCore.Identity;
using TourBooking.Application.Expactions;
using TourBooking.Domain.Entities; // AppUser burada
using System.Linq;

namespace TourBooking.Application.Features;

public sealed class LocationUpdateCommandHandler
    : IRequestHandler<LocationUpdateCommand>
{
    private readonly UserManager<AppUser> _userManager;

    public LocationUpdateCommandHandler(UserManager<AppUser> userManager)
        => _userManager = userManager;

    public async Task Handle(LocationUpdateCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new BusinessRuleValidationException("Geçersiz istek.");

        // Aralık validasyonu
        if (request.Latitude is < -90 or > 90)
            throw new BusinessRuleValidationException("Latitude -90 ile 90 arasında olmalı.");
        if (request.Longitude is < -180 or > 180)
            throw new BusinessRuleValidationException("Longitude -180 ile 180 arasında olmalı.");

        // Kullanıcıyı getir
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new BusinessRuleValidationException("Kullanıcı bulunamadı.");

        // 6 hane yuvarla + clamp
        var lat = Math.Clamp(
            Math.Round(request.Latitude, 6, MidpointRounding.AwayFromZero),
            -90d, 90d);
        var lon = Math.Clamp(
            Math.Round(request.Longitude, 6, MidpointRounding.AwayFromZero),
            -180d, 180d);

        // Değişiklik yoksa çık
        if (user.Latitude == lat && user.Longitude == lon)
            return;

        user.Latitude  = lat;   
        user.Longitude = lon;   

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new BusinessRuleValidationException(
                "Konum güncellenemedi: " + string.Join("; ", result.Errors.Select(e => e.Description)));
    }
}
