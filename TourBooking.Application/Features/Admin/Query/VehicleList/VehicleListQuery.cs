using System;
using MediatR;

namespace TourBooking.Application.Features.Admin.Query
{
    public class VehicleListQuery : IRequest<VehicleListQueryResponse> { }
}
