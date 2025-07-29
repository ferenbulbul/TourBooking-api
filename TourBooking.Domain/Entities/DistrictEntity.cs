namespace TourBooking.Domain.Entities
{
    public class DistrictEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<DistrictTranslation> Translations { get; set; }
        public Guid CityId { get; set; }
        public CityEntity City { get; set; } 
    }
}
