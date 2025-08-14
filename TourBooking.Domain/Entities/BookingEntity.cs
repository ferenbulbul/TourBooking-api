using System;
using TourBooking.Domain.Enums;

namespace TourBooking.Domain.Entities
{
    public class BookingEntity : IBaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid Id { get; set; }
        public Guid? GuideId { get; set; }
        public Guid AgencyId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid DriverId { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid FromCityId { get; set; }
        public Guid FromDistrictId { get; set; }
        public Guid TourPointId { get; set; }
        public GuideUserEntity? Guide { get; set; } = null!;
        public DateOnly StartDate { get; set; } // .NET 8 kullanıyorsan DateOnly güzel olur
        public DateOnly EndDate { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
    }
}
