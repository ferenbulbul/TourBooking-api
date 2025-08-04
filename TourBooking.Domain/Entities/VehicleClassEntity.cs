namespace TourBooking.Domain.Entities
{
    public class VehicleClassEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<VehicleClassTranslation> Translations { get; set; }
    }
}
