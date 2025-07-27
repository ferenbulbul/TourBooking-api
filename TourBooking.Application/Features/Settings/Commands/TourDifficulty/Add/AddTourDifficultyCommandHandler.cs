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
                Translations = request
                    .Translations.Select(t => new TourDifficultyTranslation
                    {
                        Title = t.Title,
                        Description = t.Description,
                        LanguageId = t.LanguageId,
                    })
                    .ToList()
            };
            await _unitOfWork.GetRepository<TourDifficultyEntity>().AddAsync(tourDifficulty);
        }
    }
}
