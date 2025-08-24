using System;

namespace TourBooking.Application.DTOs.Admin
{
    public record AdminManagementUserDto(
        Guid Id,
        string Name,
        string Surname,
        string? Email,
        string PhoneNumber
        );
}
