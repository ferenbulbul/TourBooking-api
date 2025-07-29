namespace TourBooking.Domain.Entities
{
    public class RegionEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<RegionTranslation> Translations { get; set; }
        public CountryEntity Country { get; set; }
        public Guid CountryId { get; set; }
        public ICollection<CityEntity> Cities { get; set; }
    }
}
