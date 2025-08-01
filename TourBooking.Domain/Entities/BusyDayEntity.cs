using System;

namespace TourBooking.Domain.Entities
{
    public class BusyDayEntity
    {
        public Guid Id { get; set; }
        public Guid AvailabilityId { get; set; }
        public AvailabilityEntity Availability { get; set; } = null!;
        public DateOnly Day { get; set; } // EF Co
    }
}
