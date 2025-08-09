using System;

namespace TourBooking.Application.DTOs.Mobile
{
    public record GuideTourPriceDto(
        Guid Id,
        Guid GuideId,
        Guid? CityId,
        Guid? DistrictId,
        Guid? TourPointId,
        decimal Price,
        string Currency
    );

    public record UpsertGuideTourPriceRequest(
        Guid? CityId,
        Guid? DistrictId,
        Guid? TourPointId,
        decimal Price,
        string? Currency // null ise TRY
    );

    public record GuideLanguageUpdateRequest(List<Guid> LanguageIds);
}
