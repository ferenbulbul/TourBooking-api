using MediatR;

namespace TourBooking.Application.Features;

public class MobileCityQuery : IRequest<MobileCityQueryResponse> { public Guid RegionId { get; set; } }
