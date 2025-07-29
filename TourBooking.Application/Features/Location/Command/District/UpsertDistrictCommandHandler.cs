using System;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features
{
    public class UpsertDistrictCommandHandler : IRequestHandler<UpsertDistrictCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertDistrictCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpsertDistrictCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.District(request.Id.Value);

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
                                    new DistrictTranslation
                                    {
                                        Title = newTr.Title,
                                        Description = newTr.Description,
                                        LanguageId = newTr.LanguageId
                                    }
                                );
                            }
                        }
                        existing.CityId = request.CityId;
                        await _unitOfWork.GetRepository<DistrictEntity>().UpdateAsync(existing);
                    }
                }
                else
                {
                    var city = new DistrictEntity
                    {
                        CityId = request.CityId,
                        Translations = request
                            .Translations.Select(t => new DistrictTranslation
                            {
                                Title = t.Title,
                                Description = t.Description,
                                LanguageId = t.LanguageId,
                            })
                            .ToList()
                    };
                    await _unitOfWork.GetRepository<DistrictEntity>().AddAsync(city);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
