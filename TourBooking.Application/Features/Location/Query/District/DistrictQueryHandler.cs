using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class DistrictQueryHandler : IRequestHandler<DistrictQuery, DistrictQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DistrictQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DistrictQueryResponse> Handle(
            DistrictQuery request,
            CancellationToken cancellationToken
        )
        {
            var districts = await _unitOfWork.Districts();

            if (districts == null || !districts.Any())
            {
                throw new NotFoundException("Kayıtlı bölge bulunamadı.");
            }
            var dtos = districts.Select(tt => new DistrictDto
            {
                Id = tt.Id,
                CityId = tt.CityId,
                Translations = tt
                    .Translations.Select(ttr => new TranslationDto
                    {
                        Title = ttr.Title,
                        Description = ttr.Description,
                        LanguageId = ttr.LanguageId
                    })
                    .ToList()
            });
            var response = new DistrictQueryResponse { Districts = dtos };
            return response;
        }
    }
}
