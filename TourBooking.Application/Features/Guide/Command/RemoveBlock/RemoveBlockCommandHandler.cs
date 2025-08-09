using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings
{
    public class RemoveBlockCommandHandler : IRequestHandler<RemoveBlockCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveBlockCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveBlockCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.RemoveGuideBlock(request);
        }
    }
}
