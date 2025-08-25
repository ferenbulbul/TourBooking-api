using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
    public class CustomerLocationEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public double Latitude { get; set; }   // decimal(9,6)
        public double Longitude { get; set; }   // decimal(9,6)
        public DateTime UpdatedAt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}