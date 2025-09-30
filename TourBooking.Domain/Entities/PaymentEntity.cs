using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourBooking.Domain.Enums;

namespace TourBooking.Domain.Entities
{
    public class PaymentEntity : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // İlişki
        public Guid BookingId { get; set; }
        public BookingEntity Booking { get; set; } = null!;

        // İyzico tarafı
        public string ConversationId { get; set; } = null!;
        public string Token { get; set; } = null!;
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        // Audit
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Opsiyonel: iyzico response’unu loglamak istersen
        public string? RawResponse { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } 
    }
}