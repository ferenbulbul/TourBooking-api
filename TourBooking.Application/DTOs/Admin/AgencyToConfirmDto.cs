using System;

namespace TourBooking.Application.DTOs.Admin
{
    public record AgencyToConfirmDto(
        Guid AgencyId,
        string? AuthorizedName,
        string? AuthorizedSurName,
        string? CompanyName,
        string? City,
        string? Address,
        string? Email,
        string? PhoneNumber,
        string? PhoneNumber2,
        string? TursabUrl,
        string? TaxNumber
    );
}
