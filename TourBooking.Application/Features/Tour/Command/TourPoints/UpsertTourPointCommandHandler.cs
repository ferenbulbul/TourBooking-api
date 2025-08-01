using System;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features
{
    public class UpsertTourPointCommandHandler : IRequestHandler<UpsertTourPointCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertTourPointCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            UpsertTourPointCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.TourPoint(request.Id.Value);

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
                                    new TourPointTranslation
                                    {
                                        Title = newTr.Title,
                                        Description = newTr.Description,
                                        LanguageId = newTr.LanguageId
                                    }
                                );
                            }
                        }
                        if (!string.IsNullOrEmpty(request.MainImage))
                        {
                            existing.MainImage = request.MainImage;
                        }
                        if (request.OtherImages != null && request.OtherImages.Any())
                        {
                            existing.OtherImages = request.OtherImages;
                        }
                        existing.IsActive = request.IsActive;
                        existing.IsHighlighted = request.IsHighlighted;
                        existing.CountryId = request.CountryId;
                        existing.RegionId = request.RegionId;
                        existing.CityId = request.CityId;
                        existing.DistrictId = request.DistrictId;
                        existing.TourDifficultyId = request.DifficultyId;
                        existing.TourTypeId = request.TourTypeId;


                        await _unitOfWork.GetRepository<TourPointEntity>().UpdateAsync(existing);
                    }
                }
                else
                {
                    var tourPoint = new TourPointEntity
                    {
                        MainImage = request.MainImage,
                        OtherImages = request.OtherImages,
                        IsHighlighted = request.IsHighlighted,
                        IsActive = request.IsActive,
                        CountryId = request.CountryId,
                        RegionId = request.RegionId,
                        CityId = request.CityId,
                        DistrictId = request.DistrictId,
                        TourDifficultyId = request.DifficultyId,
                        TourTypeId = request.TourTypeId,

                        Translations = request
                            .Translations.Select(t => new TourPointTranslation
                            {
                                Title = t.Title,
                                Description = t.Description,
                                LanguageId = t.LanguageId,
                            })
                            .ToList()
                    };
                    await _unitOfWork.GetRepository<TourPointEntity>().AddAsync(tourPoint);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
