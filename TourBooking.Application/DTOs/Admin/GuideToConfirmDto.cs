using System;

namespace TourBooking.Application.DTOs.Admin
{
    public record GuideToConfirmDto(
        Guid GuideId,
        string? FirstName,
        string? LastName,
        string? LicenceNumber,
        string? Email,
        string? PhoneNumber
    );
}
