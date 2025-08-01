using System;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class LegroomQueryHandler
        : IRequestHandler<LegroomQuery, LegroomQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LegroomQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LegroomQueryResponse> Handle(
            LegroomQuery request,
            CancellationToken cancellationToken
        )
        {
            var legrooms = await _unitOfWork.Legrooms();

            if (legrooms == null || !legrooms.Any())
            {
                throw new NotFoundException("Araç tipi  bulunamadı.");
            }
            var dtos = legrooms.Select(tt => new LegroomSpaceDto
            {
                Id = tt.Id,
                Translations = tt
                    .Translations.Select(ttr => new TranslationDto
                    {
                        Title = ttr.Title,
                        Description = ttr.Description,
                        LanguageId = ttr.LanguageId
                    })
                    .ToList()
            });
            var response = new LegroomQueryResponse { Legrooms = dtos };
            return response;
        }
    }
}
