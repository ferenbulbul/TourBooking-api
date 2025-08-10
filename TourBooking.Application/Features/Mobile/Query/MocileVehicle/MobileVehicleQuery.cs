using MediatR;

namespace TourBooking.Application.Features;

public class MobileVehicleQuery : IRequest<MobileVehicleQueryResponse> { public Guid Id { get; set; }}
