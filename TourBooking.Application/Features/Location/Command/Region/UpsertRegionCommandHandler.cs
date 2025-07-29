using System;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features
{
    public class UpsertRegionCommandHandler : IRequestHandler<UpsertRegionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertRegionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpsertRegionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.Region(request.Id.Value);

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
                                    new RegionTranslation
                                    {
                                        Title = newTr.Title,
                                        Description = newTr.Description,
                                        LanguageId = newTr.LanguageId
                                    }
                                );
                            }
                        }
                        existing.CountryId = request.CountryId;

                        await _unitOfWork.GetRepository<RegionEntity>().UpdateAsync(existing);
                    }
                }
                else
                {
                    var city = new RegionEntity
                    {
                        CountryId = request.CountryId,
                        Translations = request
                            .Translations.Select(t => new RegionTranslation
                            {
                                Title = t.Title,
                                Description = t.Description,
                                LanguageId = t.LanguageId,
                            })
                            .ToList()
                    };
                    await _unitOfWork.GetRepository<RegionEntity>().AddAsync(city);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
