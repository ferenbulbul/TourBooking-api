namespace TourBooking.Domain.Entities
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }
        bool IsDeleted { get; set; }
    }
}
