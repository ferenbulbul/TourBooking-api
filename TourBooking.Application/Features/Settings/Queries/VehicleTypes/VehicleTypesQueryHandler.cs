using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features.Settings.Queries;

public class VehicleTypesQueryHandler
    : IRequestHandler<VehicleTypesQuery, VehicleTypesQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public VehicleTypesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<VehicleTypesQueryResponse> Handle(
        VehicleTypesQuery request,
        CancellationToken cancellationToken
    )
    {
        // Adjust the parameters below to match the required signature of GetAllAsync
        var vehicleTypes = await _unitOfWork.VehicleTypes();

        if (vehicleTypes == null || !vehicleTypes.Any())
        {
            throw new NotFoundException("Araç tipi  bulunamadı.");
        }
        var dtos = vehicleTypes.Select(tt => new VehicleTypeDto
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
        var response = new VehicleTypesQueryResponse { VehicleTypes = dtos };
        return response;
    }
}
