using MediatR;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Queries;

public class VehicleTypesByCodeQueryHandler
    : IRequestHandler<VehicleTypesByCodeQuery, VehicleTypesByCodeQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public VehicleTypesByCodeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<VehicleTypesByCodeQueryResponse> Handle(
        VehicleTypesByCodeQuery request,
        CancellationToken cancellationToken
    )
    {
        // Adjust the parameters below to match the required signature of GetAllAsync
        var allVehicles = await _unitOfWork.VehicleTypesByCode(request.Code, cancellationToken);

        if (allVehicles == null || !allVehicles.Any())
        {
            throw new NotFoundException("Araç türleri bulunamadı.");
        }
        var response = new VehicleTypesByCodeQueryResponse();
        response.VehicleTypes = allVehicles
            .Select(v => new TourBooking.Application.DTOs.VehicleTypeDto
            {
                Id = v.Id,
                Code = v.Code,
                Title = v.Title,
                IsActive = v.IsActive,
                LanguageCode = v.LanguageCode,
            })
            .ToList();
        return response;
    }
}
