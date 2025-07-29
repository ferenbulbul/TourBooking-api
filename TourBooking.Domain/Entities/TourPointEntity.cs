using System;

namespace TourBooking.Domain.Entities
{
    public class TourPointEntity : IBaseEntity
    {
        public bool IsActive { get; set; }
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public CountryEntity Country { get; set; }
        public Guid CityId { get; set; }
        public CityEntity City { get; set; }
        public Guid DistrictId { get; set; }
        public DistrictEntity District { get; set; }
        public Guid RegionId { get; set; }
        public RegionEntity Region { get; set; }
        public Guid TourDifficultyId { get; set; }
        public TourDifficultyEntity TourDifficulty { get; set; }
        public Guid TourTypeId { get; set; }
        public TourTypeEnitity TourType { get; set; }
        public string MainImage { get; set; }
        public List<string> OtherImages { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<TourPointTranslation> Translations { get; set; }
    }
}
