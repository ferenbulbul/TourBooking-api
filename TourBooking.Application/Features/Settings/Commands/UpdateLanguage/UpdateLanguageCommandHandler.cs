using System;
using MediatR;
using TourBooking.Application.Features.Vehicle.Commands.AddVehicleBrand;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLanguageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
        {
            var language = new LanguageEntity
            {
                Id = request.Id,
                Name = request.Name,
                Code = request.Code,
                IsActive = request.IsActive,
                IsDeleted = request.IsDeleted,
                UpdatedDate = DateTime.UtcNow,
            };
            await _unitOfWork.GetRepository<LanguageEntity>().UpdateAsync(language);
        }
    }
}
