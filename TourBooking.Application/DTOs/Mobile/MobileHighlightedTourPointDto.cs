using System;

namespace TourBooking.Application.DTOs.Mobile
{
    public class MobileHighlightedTourPointDto
    {
        public Guid Id { get; set; }
        public Guid CityId { get; set; }
        public string CityName { get; set; }
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }
        public Guid RegionId { get; set; }
        public string RegionName { get; set; }
        public Guid TourTypeId { get; set; }
        public string TourTypeName { get; set; }
        public string MainImage { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
