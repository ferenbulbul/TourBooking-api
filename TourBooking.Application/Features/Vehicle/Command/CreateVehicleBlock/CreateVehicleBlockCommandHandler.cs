using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings
{
    public class CreateVehicleBlockCommandHandler : IRequestHandler<CreateVehicleBlockCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateVehicleBlockCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateVehicleBlockCommand request, CancellationToken cancellationToken)
        {
            if (request.End < request.Start)
                throw new ArgumentException("End must be >= Start");
            await _unitOfWork.CreateVehicleBlock(request);
        }
    }
}
