using System;
using MediatR;
using TourBooking.Application.Features.Vehicle.Commands.AddVehicleBrand;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Vehicle.Commands.AddVehicleType
{
    public class AddVehicleBrandCommandHandler : IRequestHandler<AddVehicleBrandCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddVehicleBrandCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            AddVehicleBrandCommand request,
            CancellationToken cancellationToken
        )
        {
            var vehicleBrand = new VehicleBrand
            {
                Name = request.Name,
                IsActive = request.IsActive,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
            };
            await _unitOfWork.GetRepository<VehicleBrand>().AddAsync(vehicleBrand);
        }
    }
}
