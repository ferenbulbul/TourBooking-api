namespace TourBooking.Application.DTOs
{
    public class VehicleDto : BaseDto
    {
        public Guid Id { get; set; }
        public string? VehicleName { get; set; }
        public Guid VehicleTypeId { get; set; }
        public Guid VehicleBrandId { get; set; }
        public Guid VehicleClassId { get; set; }
        public Guid LegRoomSpaceId { get; set; }
        public Guid SeatTypeId { get; set; }
        public int SeatCount { get; set; }
        public string LicensePlate { get; set; }
        public int ModelYear { get; set; }
        public bool IsActive { get; set; }
        public string AracResmi { get; set; }
        public string Ruhsat { get; set; }
        public string Sigorta { get; set; }
        public string Tasimacilik { get; set; }
    }
}
