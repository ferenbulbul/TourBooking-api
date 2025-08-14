using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings
{
    public class RemoveVehicleBlockCommandHandler : IRequestHandler<RemoveVehicleBlockCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveVehicleBlockCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveVehicleBlockCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.RemoveVehicleBlock(request);
        }
    }
}
