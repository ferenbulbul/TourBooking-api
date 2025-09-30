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
        public AgencyUserEntity? Agency { get; set; } = null!;
        public Guid VehicleId { get; set; }
        public VehicleEntity? Vehicle { get; set; } = null!;
        public Guid DriverId { get; set; }
        public DriverEntity? Driver { get; set; } = null!;
        public decimal? GuidePrice { get; set; }
        public decimal TourPrice { get; set; }
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
        public string? LocationDescription { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? DepartureTime { get; set; }
        public ICollection<PaymentEntity> Payments { get; set; } = new List<PaymentEntity>();
    }
}
// public class BookingEntity : IBaseEntity
// {
//     public Guid Id { get; set; }
//     public Guid AgencyId { get; set; }
//     public Guid VehicleId { get; set; }
//     public Guid DriverId { get; set; }
//     public Guid CustomerId { get; set; }

//     public decimal TotalPrice { get; set; }
//     public DateOnly StartDate { get; set; }
//     public DateOnly EndDate { get; set; }
//     public BookingStatus Status { get; set; } = BookingStatus.Pending;

//     public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
//     public DateTime? UpdatedAt { get; set; }
//     public bool IsDeleted { get; set; }

//     // Navigations
//     public AgencyUserEntity? Agency { get; set; } = null!;
//     public VehicleEntity? Vehicle { get; set; } = null!;
//     public DriverEntity? Driver { get; set; } = null!;
//     public GuideUserEntity? Guide { get; set; } = null!;
//     public ICollection<PaymentEntity> Payments { get; set; } = new List<PaymentEntity>();
// }

