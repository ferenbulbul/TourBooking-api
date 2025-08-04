using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class MobileHighlightedTourPointQueryHandler
    : IRequestHandler<MobileHighlightedTourPointQuery, MobileHighlightedTourPointQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public MobileHighlightedTourPointQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MobileHighlightedTourPointQueryResponse> Handle(
        MobileHighlightedTourPointQuery request,
        CancellationToken cancellationToken
    )
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        // Adjust the parameters below to match the required signature of GetAllAsync
        var points = await _unitOfWork.HighlightedTourPoints();

        if (points == null || !points.Any())
        {
            throw new NotFoundException("Tur noktası bulunamadı.");
        }
        var dtos = points.Select(tt => new MobileHighlightedTourPointDto
        {
            CityId = tt.CityId,
            CityName = tt.City.Translations.FirstOrDefault(t => t.Language.Code == culture)?.Title,
            Id = tt.Id,
            MainImage = tt.MainImage,
            Title = tt.Translations.FirstOrDefault(t => t.Language.Code == culture)?.Title,
            Description = tt
                .Translations?.FirstOrDefault(t => t.Language.Code == culture)
                ?.Description,
            CountryId = tt.CountryId,
            CountryName = tt
                .Country.Translations.FirstOrDefault(t => t.Language.Code == culture)
                ?.Title,
            RegionId = tt.RegionId,
            RegionName = tt
                .Region.Translations.FirstOrDefault(t => t.Language.Code == culture)
                ?.Title,
            TourTypeId = tt.TourTypeId,
            TourTypeName = tt
                .TourType.Translations.FirstOrDefault(t => t.Language.Code == culture)
                ?.Title,
        });
        var response = new MobileHighlightedTourPointQueryResponse { TourPoints = dtos };
        return response;
    }
}
