using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
    public class CustomerUser : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Id))]
        public virtual AppUser AppUser { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string PhoneNumber { get; set; }
    }
}
