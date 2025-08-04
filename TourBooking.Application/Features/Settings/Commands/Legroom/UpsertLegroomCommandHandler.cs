using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features
{
    public class UpsertLegroomCommandHandler : IRequestHandler<UpsertLegroomCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertLegroomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            UpsertLegroomCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.Legroom(request.Id.Value);

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
                                    new LegroomSpaceTranslation
                                    {
                                        Title = newTr.Title,
                                        Description = newTr.Description,
                                        LanguageId = newTr.LanguageId
                                    }
                                );
                            }
                        }

                        await _unitOfWork.GetRepository<LegroomSpaceEntity>().UpdateAsync(existing);
                    }
                }
                else
                {
                    var legroom = new LegroomSpaceEntity
                    {
                        Translations = request
                            .Translations.Select(t => new LegroomSpaceTranslation
                            {
                                Title = t.Title,
                                Description = t.Description,
                                LanguageId = t.LanguageId,
                            })
                            .ToList()
                    };
                    await _unitOfWork.GetRepository<LegroomSpaceEntity>().AddAsync(legroom);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
