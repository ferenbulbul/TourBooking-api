using System;

namespace TourBooking.Domain.Entities
{
    public class TourPointEntity : IBaseEntity
    {
        public bool IsActive { get; set; }
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public Guid CityId { get; set; }
        public Guid DistrictId { get; set; }
        public Guid RegionId { get; set; }
        public TourDifficultyEntity TourDifficulty { get; set; }
        public TourTypeEnitity TourType { get; set; }
        public string MainImageUrl { get; set; }
        public ICollection<string> OtherImagesUrlList { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<TourPointTranslation> Translations { get; set; }
    }
}
