using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class AddTourDifficultyCommandHandler : IRequestHandler<AddTourDifficultyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddTourDifficultyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            AddTourDifficultyCommand request,
            CancellationToken cancellationToken
        )
        {
            var tourDifficulty = new TourDifficultyEntity
            {
                Name = request.Name,
                IsActive = request.IsActive,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
            };
            await _unitOfWork.GetRepository<TourDifficultyEntity>().AddAsync(tourDifficulty);
        }
    }
}
