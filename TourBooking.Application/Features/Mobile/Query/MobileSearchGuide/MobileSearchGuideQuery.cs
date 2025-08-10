using MediatR;

namespace TourBooking.Application.Features;

public class MobileSearchGuideQuery : IRequest<MobileSearchGuideQueryResponse>
{
    public Guid CityId { get; set; }
    public Guid DistrictId { get; set; }
    public Guid TourPointId { get; set; }
    public DateOnly Date { get; set; }
}
