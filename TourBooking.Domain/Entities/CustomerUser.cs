using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
    public class CustomerUser : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string PhoneNumber { get; set; }
    }
}