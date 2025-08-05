using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class VehicleClassQueryHandler
        : IRequestHandler<VehicleClassQuery, VehicleClassQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleClassQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<VehicleClassQueryResponse> Handle(
            VehicleClassQuery request,
            CancellationToken cancellationToken
        )
        {
            var vehicleClasses = await _unitOfWork.VehicleClasses();

            if (vehicleClasses == null || !vehicleClasses.Any())
            {
                throw new NotFoundException("Araç tipi  bulunamadı.");
            }
            var dtos = vehicleClasses.Select(tt => new VehicleClassDto
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
            var response = new VehicleClassQueryResponse { VehicleClasses = dtos };
            return response;
        }
    }
}
