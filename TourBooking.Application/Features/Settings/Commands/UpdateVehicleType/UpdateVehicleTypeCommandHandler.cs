using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Vehicle.Commands.AddVehicleType
{
    public class UpdateVehicleTypeCommandHandler : IRequestHandler<UpdateVehicleTypeCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVehicleTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            UpdateVehicleTypeCommand request,
            CancellationToken cancellationToken
        )
        {
            var vehicleType = new VehicleType
            {
                Id = request.Id,
                Name = request.Name,
                IsActive = request.IsActive,
                IsDeleted = request.IsDeleted,
                UpdatedDate = DateTime.UtcNow,
            };
            await _unitOfWork.GetRepository<VehicleType>().UpdateAsync(vehicleType);
        }
    }
}
