using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features.Authentication.Queries.IsApproved
{
    public class IsApprovedQueryHandler : IRequestHandler<IsApprovedQuery, IsApprovedQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public IsApprovedQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IsApprovedQueryResponse> Handle(
            IsApprovedQuery request,
            CancellationToken cancellationToken
        )
        {
            IsApprovedQueryResponse response =
                new() { IsApproved = await _unitOfWork.IsUserApproved(request) };

            return response;
        }
    }
}
