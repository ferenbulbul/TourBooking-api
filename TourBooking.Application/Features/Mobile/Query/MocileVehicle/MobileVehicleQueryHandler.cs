using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class MobileVehicleQueryHandler
    : IRequestHandler<MobileVehicleQuery, MobileVehicleQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public MobileVehicleQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MobileVehicleQueryResponse> Handle(
        MobileVehicleQuery request,
        CancellationToken cancellationToken

    )
    {
        var Vehicle = await _unitOfWork.MobileDetailVehicle(request.Id);

        if (Vehicle == null )
        {
            throw new NotFoundException("Araç Bulunamadı  bulunamadı.");
        }
        
        var response = new MobileVehicleQueryResponse { VehicleDtos = Vehicle };
        return response;
    }

}
