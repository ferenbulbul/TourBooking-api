using System;
using MediatR;

namespace TourBooking.Application.Features.Admin.Query.AgenciesToConfirm
{
    public class CallCenterAgentsQuery : IRequest<CallCenterAgentsQueryResponse> { }
}
