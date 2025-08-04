using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features
{
    public class UpsertVehicleCommandHandler : IRequestHandler<UpsertVehicleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertVehicleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpsertVehicleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.Vehicle(request.Id.Value);

                    if (existing != null)
                    {
                        existing.AracResmi = request.AracResmi;

                        existing.LegRoomSpaceId = request.LegRoomSpaceId;
                        existing.LicensePlate = request.LicensePlate;
                        existing.ModelYear = request.ModelYear;
                        existing.Ruhsat = request.Ruhsat;
                        existing.SeatCount = request.SeatCount;
                        existing.SeatTypeId = request.SeatTypeId;
                        existing.Sigorta = request.Sigorta;
                        existing.Tasimacilik = request.Tasimacilik;
                        existing.VehicleBrandId = request.VehicleBrandId;
                        existing.VehicleClassId = request.VehicleClassId;
                        existing.VehicleName = request.VehicleName;
                        existing.VehicleTypeId = request.VehicleTypeId;
                        await _unitOfWork.GetRepository<VehicleEntity>().UpdateAsync(existing);
                    }
                }
                else
                {
                    var driver = new VehicleEntity
                    {
                        LegRoomSpaceId = request.LegRoomSpaceId,
                        LicensePlate = request.LicensePlate,
                        ModelYear = request.ModelYear,
                        Ruhsat = request.Ruhsat,
                        SeatCount = request.SeatCount,
                        SeatTypeId = request.SeatTypeId,
                        Sigorta = request.Sigorta,
                        Tasimacilik = request.Tasimacilik,
                        VehicleBrandId = request.VehicleBrandId,
                        VehicleClassId = request.VehicleClassId,
                        VehicleName = request.VehicleName,
                        VehicleTypeId = request.VehicleTypeId,
                        AracResmi = request.AracResmi,
                        AgencyId = request.AgencyId
                    };
                    await _unitOfWork.GetRepository<VehicleEntity>().AddAsync(driver);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
