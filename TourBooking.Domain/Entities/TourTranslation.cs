using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
    public class TourTranslation : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid TourId { get; set; }
        public TourTypeEnitity Tour { get; set; }
        public Guid LanguageId { get; set; }
        public LanguageEntity Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}