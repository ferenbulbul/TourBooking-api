using System;

namespace TourBooking.Application.DTOs.Mobile
{
    public class MobileDetailedSearchResultDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public Guid TourTypeId { get; set; }
        public string? TourTypeName { get; set; }

        // Zorluk
        public Guid TourDifficultyId { get; set; }
        public string? TourDifficultyName { get; set; }

        // Konum
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        public Guid RegionId { get; set; }
        public string? RegionName { get; set; }

        public Guid CityId { get; set; }
        public string? CityName { get; set; }

        public Guid DistrictId { get; set; }
        public string? DistrictName { get; set; }

        // Görseller
        public string? MainImage { get; set; }
        public List<string>? OtherImages { get; set; }
    }
}
