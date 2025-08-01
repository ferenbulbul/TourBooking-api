using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
    public class VehicleEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public string? VehicleName { get; set; }
        public Guid VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
        public Guid VehicleBrandId { get; set; }
        public VehicleBrand VehicleBrand { get; set; }
        public Guid VehicleClassId { get; set; }
        public VehicleClassEntity VehicleClass { get; set; }
        public Guid LegRoomSpaceId { get; set; }
        public LegroomSpaceEntity LegRoomSpace { get; set; }
        public Guid SeatTypeId { get; set; }
        public SeatTypeEntity SeatType { get; set; }
        public int SeatCount { get; set; }
        public string LicensePlate { get; set; }
        public int ModelYear { get; set; }
        public bool IsActive { get; set; }
        public string AracResmi { get; set; }
        public string Ruhsat { get; set; }
        public string Sigorta { get; set; }
        public string Tasimacilik { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<AvailabilityEntity> AvailabilityList { get; set; }
    }
}
