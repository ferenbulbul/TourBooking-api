using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features.Admin.Query.AgenciesToConfirm
{
    public class GuidesToConfirmQueryHandler
        : IRequestHandler<GuidesToConfirmQuery, GuidesToConfirmQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GuidesToConfirmQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GuidesToConfirmQueryResponse> Handle(
            GuidesToConfirmQuery request,
            CancellationToken cancellationToken
        )
        {
            return new GuidesToConfirmQueryResponse
            {
                Guides = await _unitOfWork.GetGuidesToConfirm()
            };
        }
    }
}
