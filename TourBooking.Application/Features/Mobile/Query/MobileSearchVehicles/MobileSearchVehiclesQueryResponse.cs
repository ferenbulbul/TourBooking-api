using TourBooking.Application.DTOs.Mobile;

namespace TourBooking.Application.Features;

public class MobileSearchVehiclesQueryResponse
{
    public IEnumerable<MobileSearchVehicleDto> Vehicles { get; set; }
}
