namespace TourBooking.Domain.Entities
{
    public class TourDifficultyEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<TourDifficultyTranslation> Translations { get; set; }
    }
}
