namespace TourBooking.Application.DTOs
{
    public class TourPointDto : BaseDto
    {
        public bool? IsHighlighted { get; set; }
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public Guid RegionId { get; set; }
        public Guid CityId { get; set; }
        public Guid DistrictId { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid TourTypeId { get; set; }
        public string? MainImage { get; set; }
        public List<string> OtherImages { get; set; }
        public List<TranslationDto> Translations { get; set; }

        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public string DifficultyName { get; set; }
        public string TourTypeName { get; set; }
    }
}
