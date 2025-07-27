using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features;

public class TourTypesQueryHandler : IRequestHandler<TourTypesQuery, TourTypesQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public TourTypesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TourTypesQueryResponse> Handle(
        TourTypesQuery request,
        CancellationToken cancellationToken
    )
    {
        // Adjust the parameters below to match the required signature of GetAllAsync
        var allTourType = await _unitOfWork.TourTypes();

        if (allTourType == null || !allTourType.Any())
        {
            throw new NotFoundException("Tur tipi  bulunamadı.");
        }
        var dtos = allTourType.Select(tt => new TourTypeDto
        {
            Id = tt.Id,
            MainImageUrl = tt.MainImageUrl,
            ThumbImageUrl = tt.ThumbImageUrl,
            Translations = tt
                .Translations.Select(ttr => new TranslationDto
                {
                    Title = ttr.Title,
                    Description = ttr.Description,
                    LanguageId = ttr.LanguageId
                })
                .ToList()
        });
        var response = new TourTypesQueryResponse { TourTypeDtos = dtos };
        return response;
    }
}
