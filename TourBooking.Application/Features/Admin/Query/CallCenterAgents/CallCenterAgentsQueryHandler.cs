using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features.Admin.Query.AgenciesToConfirm
{
    public class CallCenterAgentsQueryHandler
        : IRequestHandler<CallCenterAgentsQuery, CallCenterAgentsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CallCenterAgentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CallCenterAgentsQueryResponse> Handle(
            CallCenterAgentsQuery request,
            CancellationToken cancellationToken
        )
        {
            return new CallCenterAgentsQueryResponse
            {
                Agents = await _unitOfWork.GetCallCenterAgents()
            };
        }
    }
}
