using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
    public class DriverLocationEntity :IBaseEntity
    {
        public Guid Id{ get; set; }
        public double Latitude { get; set; }   
        public double Longitude { get; set; }   
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}