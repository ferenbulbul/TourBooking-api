using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class CountryQueryHandler : IRequestHandler<CountryQuery, CountryQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CountryQueryResponse> Handle(
            CountryQuery request,
            CancellationToken cancellationToken
        )
        {
            var countries = await _unitOfWork.Countries();

            if (countries == null || !countries.Any())
            {
                throw new NotFoundException("Kayıtlı ülke  bulunamadı.");
            }
            var dtos = countries.Select(tt => new CountryDto
            {
                Id = tt.Id,
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
            var response = new CountryQueryResponse { Countries = dtos };
            return response;
        }
    }
}
