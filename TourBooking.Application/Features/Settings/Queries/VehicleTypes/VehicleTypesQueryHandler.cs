using MediatR;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

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
        var allVehicles = await _unitOfWork.GetRepository<VehicleType>().GetAllAsync();

        if (allVehicles == null || !allVehicles.Any())
        {
            throw new NotFoundException("Araç türleri bulunamadı.");
        }
        var response = new VehicleTypesQueryResponse();
        response.VehicleTypes = allVehicles
            .Select(v => new TourBooking.Application.DTOs.VehicleTypeDto
            {
                Id = v.Id,
                Name = v.Name,
                IsActive = v.IsActive,
            })
            .ToList();
        return response;
    }
}
