using System;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class CityQueryHandler : IRequestHandler<CityQuery, CityQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CityQueryResponse> Handle(
            CityQuery request,
            CancellationToken cancellationToken
        )
        {
            var cities = await _unitOfWork.Cities();

            if (cities == null || !cities.Any())
            {
                throw new NotFoundException("Kayıtlı şehir  bulunamadı.");
            }
            var dtos = cities.Select(tt => new CityDto
            {
                Id = tt.Id,
                RegionId = tt.RegionId,
                Translations = tt
                    .Translations.Select(ttr => new TranslationDto
                    {
                        Title = ttr.Title,
                        Description = ttr.Description,
                        LanguageId = ttr.LanguageId
                    })
                    .ToList()
            });
            var response = new CityQueryResponse { Cities = dtos };
            return response;
        }
    }
}
