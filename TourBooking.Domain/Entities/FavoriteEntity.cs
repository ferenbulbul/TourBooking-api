using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
    public class FavoriteEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerUser Customer { get; set; }

        public Guid TourPointId { get; set; }
        public TourPointEntity TourPoint { get; set; }

    }
}