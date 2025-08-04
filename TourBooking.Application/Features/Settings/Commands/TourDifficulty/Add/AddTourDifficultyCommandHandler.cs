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
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.TourDifficulty(request.Id.Value);

                    if (existing != null)
                    {
                        foreach (var newTr in request.Translations)
                        {
                            var existingTr = existing.Translations.FirstOrDefault(t =>
                                t.LanguageId == newTr.LanguageId
                            );

                            if (existingTr != null)
                            {
                                existingTr.Title = newTr.Title;
                                existingTr.Description = newTr.Description;
                            }
                            else
                            {
                                existing.Translations.Add(
                                    new TourDifficultyTranslation
                                    {
                                        Title = newTr.Title,
                                        Description = newTr.Description,
                                        LanguageId = newTr.LanguageId
                                    }
                                );
                            }
                        }

                        await _unitOfWork
                            .GetRepository<TourDifficultyEntity>()
                            .UpdateAsync(existing);
                    }
                }
                else
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
                    await _unitOfWork
                        .GetRepository<TourDifficultyEntity>()
                        .AddAsync(tourDifficulty);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
