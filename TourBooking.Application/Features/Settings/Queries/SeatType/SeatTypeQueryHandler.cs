using System;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class SeatTypeQueryHandler
        : IRequestHandler<SeatTypeQuery, SeatTypeQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SeatTypeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SeatTypeQueryResponse> Handle(
            SeatTypeQuery request,
            CancellationToken cancellationToken
        )
        {
            var seatTypes = await _unitOfWork.SeatTypes();

            if (seatTypes == null || !seatTypes.Any())
            {
                throw new NotFoundException("Araç tipi  bulunamadı.");
            }
            var dtos = seatTypes.Select(tt => new SeatTypeDto
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
            var response = new SeatTypeQueryResponse { SeatTypes = dtos };
            return response;
        }
    }
}
