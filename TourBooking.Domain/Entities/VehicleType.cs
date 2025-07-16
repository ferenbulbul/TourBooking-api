using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
    public class VehicleType : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}