using MediatR;

namespace TourBooking.Application.Features.Settings.Queries;

public class VehicleTypesByCodeQuery : IRequest<VehicleTypesByCodeQueryResponse>
{
    public string Code { get; set; }
}