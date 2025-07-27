using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class AddVehicleTypeCommandHandler : IRequestHandler<AddVehicleTypeCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddVehicleTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle2(
            AddVehicleTypeCommand request,
            CancellationToken cancellationToken
        )
        {
            var vehicleType = new VehicleType
            {
                Code = request.Code,
                Title = request.Title,
                IsActive = request.IsActive,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                LanguageCode = request.LanguageCode,
            };
            await _unitOfWork.GetRepository<VehicleType>().AddAsync(vehicleType);
        }

        Task IRequestHandler<AddVehicleTypeCommand>.Handle(
            AddVehicleTypeCommand request,
            CancellationToken cancellationToken
        )
        {
            return Handle2(request, cancellationToken);
        }
    }
}
