namespace TourBooking.Application.DTOs.Mobile
{
    public class MobileTourPointDetailDto
    {
        public Guid Id { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string DistrictName { get; set; }
        public string TourTypeName { get; set; }
        public string TourDifficultyName { get; set; }
        public string MainImage { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> OtherImages { get; set; }
        public List<MobileCityDto> Cities { get; set; }
        public List<MobileDistrictDto2> Districts { get; set; }
    }
}
