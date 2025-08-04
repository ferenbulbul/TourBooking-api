namespace TourBooking.Domain.Entities
{
    public class LegroomSpaceEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<LegroomSpaceTranslation> Translations { get; set; }
    }
}
