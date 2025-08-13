using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features.Admin.Command.ConfirmGuide
{
    public class ConfirmGuideCommandHandler : IRequestHandler<ConfirmGuideCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmGuideCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ConfirmGuideCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.ConfirmGuide(request.Id);
        }
    }
}
