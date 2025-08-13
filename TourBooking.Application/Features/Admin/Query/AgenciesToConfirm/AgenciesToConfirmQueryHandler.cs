using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features.Admin.Query.AgenciesToConfirm
{
    public class AgenciesToConfirmQueryHandler
        : IRequestHandler<AgenciesToConfirmQuery, AgenciesToConfirmQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AgenciesToConfirmQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AgenciesToConfirmQueryResponse> Handle(
            AgenciesToConfirmQuery request,
            CancellationToken cancellationToken
        )
        {
            return new AgenciesToConfirmQueryResponse
            {
                Agencies = await _unitOfWork.GetAgenciesToConfirm()
            };
        }
    }
}
