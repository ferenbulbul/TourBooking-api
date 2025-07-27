using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
    public class VehicleType : IBaseEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public string LanguageCode { get; set; }
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}