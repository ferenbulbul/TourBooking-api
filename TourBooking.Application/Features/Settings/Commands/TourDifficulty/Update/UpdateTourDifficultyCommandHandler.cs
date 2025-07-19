using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class UpdateTourDifficultyCommandHandler : IRequestHandler<UpdateTourDifficultyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTourDifficultyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            UpdateTourDifficultyCommand request,
            CancellationToken cancellationToken
        )
        {
            var tourDifficulty = new TourDifficultyEntity
            {
                Id = request.Id,
                Name = request.Name,
                IsActive = request.IsActive,
                IsDeleted = request.IsDeleted,
                UpdatedDate = DateTime.UtcNow,
            };
            await _unitOfWork.GetRepository<TourDifficultyEntity>().UpdateAsync(tourDifficulty);
        }
    }
}
