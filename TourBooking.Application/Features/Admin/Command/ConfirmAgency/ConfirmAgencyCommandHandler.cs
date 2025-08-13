using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features.Admin.Command.ConfirmAgency
{
    public class ConfirmAgencyCommandHandler : IRequestHandler<ConfirmAgencyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmAgencyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ConfirmAgencyCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.ConfirmAgency(request.Id);
        }
    }
}
