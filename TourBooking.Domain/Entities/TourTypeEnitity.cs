using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
    public class TourTypeEnitity : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string MainImageUrl { get; set; }
        public string ThumbImageUrl { get; set; }
        public ICollection<TourTranslation> Translations { get; set; }
    }
}