using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class UpsertCityCommandHandler : IRequestHandler<UpsertCityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertCityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpsertCityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.City(request.Id.Value);

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
                                    new CityTranslation
                                    {
                                        Title = newTr.Title,
                                        Description = newTr.Description,
                                        LanguageId = newTr.LanguageId
                                    }
                                );
                            }
                        }
                        existing.RegionId = request.RegionId;

                        await _unitOfWork.GetRepository<CityEntity>().UpdateAsync(existing);
                    }
                }
                else
                {
                    var city = new CityEntity
                    {
                        RegionId = request.RegionId,
                        Translations = request
                            .Translations.Select(t => new CityTranslation
                            {
                                Title = t.Title,
                                Description = t.Description,
                                LanguageId = t.LanguageId,
                            })
                            .ToList()
                    };
                    await _unitOfWork.GetRepository<CityEntity>().AddAsync(city);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
