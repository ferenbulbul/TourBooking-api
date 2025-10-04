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


        
        public decimal? GuidePrice { get; set; }
        public decimal TourPrice { get; set; }
        public decimal TotalPrice { get; set; }
      
        public Guid TourPointId { get; set; }

        public DateOnly StartDate { get; set; } // .NET 8 kullanıyorsan DateOnly güzel olur
        public DateOnly EndDate { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerUser Customer { get; set; } = null!;
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public string? LocationDescription { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? DepartureTime { get; set; }

        public Guid TourRoutePriceId { get; set; }
        public TourRoutePriceEntity TourRoutePrice { get; set; } = default!;

        public Guid? GuideTourPriceId { get; set; }
        public GuideTourPriceEntity? GuideTourPrice { get; set; }
        
        public ICollection<PaymentEntity> Payments { get; set; } = new List<PaymentEntity>();
    }
}


