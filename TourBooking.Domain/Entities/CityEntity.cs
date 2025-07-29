using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
    public class CityEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<CityTranslation> Translations { get; set; }
        public RegionEntity Region { get; set; }
        public Guid RegionId { get; set; }
        public ICollection<DistrictEntity> Districts { get; set; }
    }
}
