using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class RegionQueryHandler : IRequestHandler<RegionQuery, RegionQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegionQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RegionQueryResponse> Handle(
            RegionQuery request,
            CancellationToken cancellationToken
        )
        {
            var regions = await _unitOfWork.Regions();

            if (regions == null || !regions.Any())
            {
                throw new NotFoundException("Kayıtlı bölge bulunamadı.");
            }
            var dtos = regions.Select(tt => new RegionDto
            {
                Id = tt.Id,
                CountryId = tt.CountryId,
                CountryName = tt
                    .Country.Translations.FirstOrDefault(x => x.Language.Code == "tr")
                    .Title,
                Translations = tt
                    .Translations.Select(ttr => new TranslationDto
                    {
                        Title = ttr.Title,
                        Description = ttr.Description,
                        LanguageId = ttr.LanguageId,
                        LanguageName = ttr.Language.Name
                    })
                    .ToList()
            });
            var response = new RegionQueryResponse { Regions = dtos };
            return response;
        }
    }
}
