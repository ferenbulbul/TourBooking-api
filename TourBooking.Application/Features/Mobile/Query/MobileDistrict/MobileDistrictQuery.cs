using MediatR;

namespace TourBooking.Application.Features;

public class MobileDistrictQuery : IRequest<MobileDistrictQueryResponse>
{
    public Guid CityId { get; set; }
}
