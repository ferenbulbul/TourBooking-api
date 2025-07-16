using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
     public class EmailVerificationCode:IBaseEntity
    {
        public Guid UserId { get; set; } 
        public string Code { get; set; } 
        public DateTime ExpiryDate { get; set; } 
        public bool IsUsed { get; set; }

        public virtual AppUser User { get; set; }
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}