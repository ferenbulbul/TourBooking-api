using System;

namespace TourBooking.Domain.Entities
{
    public class VehicleBrand : IBaseEntity
    {
        
       public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<VehicleBrandTranslation> Translations { get; set; }
    }
}
