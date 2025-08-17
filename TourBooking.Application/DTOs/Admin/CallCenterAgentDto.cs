using System;

namespace TourBooking.Application.DTOs.Admin
{
    public record CallCenterAgentDto(Guid Id, string? Email, DateTime? CreateDate, string? Firm);
}
