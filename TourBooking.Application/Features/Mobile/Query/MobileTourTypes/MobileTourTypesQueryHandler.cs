using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class MobileTourTypesQueryHandler
    : IRequestHandler<MobileTourTypesQuery, MobileTourTypesQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public MobileTourTypesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MobileTourTypesQueryResponse> Handle(
        MobileTourTypesQuery request,
        CancellationToken cancellationToken
    )
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        // Adjust the parameters below to match the required signature of GetAllAsync
        var allTourType = await _unitOfWork.TourTypes();

        if (allTourType == null || !allTourType.Any())
        {
            throw new NotFoundException("Tur tipi  bulunamadı.");
        }
        var dtos = allTourType.Select(tt => new MobileTourTypeDto
        {
            Id = tt.Id,
            MainImageUrl = tt.MainImageUrl,
            ThumbImageUrl = tt.ThumbImageUrl,
            Title = tt
                .Translations?.Where(ttr => ttr.Language.Code == culture)
                .Select(ttr => new { ttr.Title, })
                .SingleOrDefault()
                ?.Title,
            Description = tt
                .Translations?.Where(ttr => ttr.Language.Code == culture)
                .Select(ttr => new { ttr.Description, })
                .SingleOrDefault()
                ?.Description,
        });
        var response = new MobileTourTypesQueryResponse { TourTypes = dtos };
        return response;
    }
}
