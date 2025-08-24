using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class TourPointsQueryHandler : IRequestHandler<TourPointsQuery, TourPointsQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public TourPointsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TourPointsQueryResponse> Handle(
        TourPointsQuery request,
        CancellationToken cancellationToken
    )
    {
        // Adjust the parameters below to match the required signature of GetAllAsync
        var tourPoints = await _unitOfWork.TourPoints();
        var languageCode = "tr";

        if (tourPoints == null || !tourPoints.Any())
        {
            throw new NotFoundException("Tur noktası bulunamadı.");
        }
        var dtos = tourPoints.Select(tt => new TourPointDto
        {
            Id = tt.Id,
            MainImage = tt.MainImage,
            OtherImages = tt.OtherImages,
            Translations = tt
                .Translations.Select(ttr => new TranslationDto
                {
                    Title = ttr.Title,
                    Description = ttr.Description,
                    LanguageId = ttr.LanguageId
                })
                .ToList(),

            CityId = tt.CityId,
            CountryId = tt.CountryId,
            DifficultyId = tt.TourDifficultyId,
            DistrictId = tt.DistrictId,
            RegionId = tt.RegionId,
            TourTypeId = tt.TourTypeId,
            IsActive = tt.IsActive,
            IsHighlighted = tt.IsHighlighted,
            Latitude=tt.Lat,
            Longitude=tt.Long,

            CountryName = tt
                .Country.Translations.FirstOrDefault(x => x.Language.Code == languageCode)
                .Title,
            RegionName = tt
                .Region.Translations.FirstOrDefault(x => x.Language.Code == languageCode)
                .Title,
            CityName = tt
                .City?.Translations.FirstOrDefault(x => x.Language.Code == languageCode)
                .Title,
            DistrictName = tt
                .District.Translations.FirstOrDefault(x => x.Language.Code == languageCode)
                .Title,
            DifficultyName = tt
                .TourDifficulty.Translations.FirstOrDefault(x => x.Language.Code == languageCode)
                .Title,
            TourTypeName = tt
                .TourType.Translations.FirstOrDefault(x => x.Language.Code == languageCode)
                .Title,
        
        });
        var response = new TourPointsQueryResponse { TourPoints = dtos };
        return response;
    }
}
