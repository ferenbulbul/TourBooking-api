using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class UpdateVehicleBrandCommandHandler : IRequestHandler<UpdateVehicleBrandCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVehicleBrandCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            UpdateVehicleBrandCommand request,
            CancellationToken cancellationToken
        )
        {
            var vehicleBrand = new VehicleBrand
            {
                Id = request.Id,
                Name = request.Name,
                IsActive = request.IsActive,
                IsDeleted = request.IsDeleted,
                UpdatedDate = DateTime.UtcNow,
            };
            await _unitOfWork.GetRepository<VehicleBrand>().UpdateAsync(vehicleBrand);
        }
    }
}
