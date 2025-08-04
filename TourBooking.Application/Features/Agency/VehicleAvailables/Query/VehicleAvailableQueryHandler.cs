using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class VehicleAvailableQueryHandler
    : IRequestHandler<VehicleAvailableQuery, VehicleAvailableQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public VehicleAvailableQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<VehicleAvailableQueryResponse> Handle(
        VehicleAvailableQuery request,
        CancellationToken cancellationToken
    )
    {
        var available = await _unitOfWork.VehicleAvailabilityByVehicleId(request.VehicleId);

        if (available == null)
        {
            throw new NotFoundException("Araca ait müsaitlik bulunamadı.");
        }
        var dto = new AvailableDto { Id = available.Id, VehicleId = available.VehicleId, };
        var response = new VehicleAvailableQueryResponse { Available = dto };
        return response;
    }
}
