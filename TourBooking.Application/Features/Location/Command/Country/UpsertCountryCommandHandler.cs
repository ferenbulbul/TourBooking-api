using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class UpsertCountryCommandHandler : IRequestHandler<UpsertCountryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertCountryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpsertCountryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.Country(request.Id.Value);

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
                                    new CountryTranslation
                                    {
                                        Title = newTr.Title,
                                        Description = newTr.Description,
                                        LanguageId = newTr.LanguageId
                                    }
                                );
                            }
                        }

                        await _unitOfWork.GetRepository<CountryEntity>().UpdateAsync(existing);
                    }
                }
                else
                {
                    var city = new CountryEntity
                    {
                        Translations = request
                            .Translations.Select(t => new CountryTranslation
                            {
                                Title = t.Title,
                                Description = t.Description,
                                LanguageId = t.LanguageId,
                            })
                            .ToList()
                    };
                    await _unitOfWork.GetRepository<CountryEntity>().AddAsync(city);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
