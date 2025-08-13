using System;

namespace TourBooking.Application.DTOs.Admin
{
    public record GuideToConfirmDto(
        Guid GuideId,
        string? FirstName,
        string? LastName,
        string? Email,
        string? PhoneNumber,
        string? DomesticUrl,
        string? RegionalUrl
    );
}
