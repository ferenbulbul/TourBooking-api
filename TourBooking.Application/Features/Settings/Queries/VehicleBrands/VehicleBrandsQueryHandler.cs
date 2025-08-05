using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class VehicleBrandsQueryHandler
        : IRequestHandler<VehicleBrandsQuery, VehicleBrandsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleBrandsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<VehicleBrandsQueryResponse> Handle(
            VehicleBrandsQuery request,
            CancellationToken cancellationToken
        )
        {
            var vehicleBrands = await _unitOfWork.VehicleBrands();

            if (vehicleBrands == null || !vehicleBrands.Any())
            {
                throw new NotFoundException("Araç tipi  bulunamadı.");
            }
            var dtos = vehicleBrands.Select(tt => new VehicleBrandDto
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
            var response = new VehicleBrandsQueryResponse { VehicleBrands = dtos };
            return response;
        }
    }
}
