using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features;

public class TourQueryHandler : IRequestHandler<TourQuery, TourQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public TourQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TourQueryResponse> Handle(
        TourQuery request,
        CancellationToken cancellationToken
    )
    {
        // Adjust the parameters below to match the required signature of GetAllAsync
        var tours = await _unitOfWork.Tours();
        var languageCode = "tr";

        if (tours == null || !tours.Any())
        {
            throw new NotFoundException("Tur noktası bulunamadı.");
        }
        var dtos = tours.Select(tt => new TourDto
        {
            Id = tt.Id,
            TourPointId = tt.TourPointId,
            TourPointName = tt.TourPoint.Translations.FirstOrDefault(x => x.Language.Code == languageCode).Title,
            Pricings = tt
                .PricingEntity.Select(ttr => new PricingDto
                {
                    CityId = ttr.CityId,
                    CountryId = ttr.CountryId,
                    DistrictId = ttr.DistrictId,
                    RegionId = ttr.RegionId,
                    DriverId = ttr.DriverId,
                    VehicleId = ttr.VehicleId,
                    Id = ttr.Id,
                    Price = ttr.Price,
                    CountryName = ttr
                        .Country.Translations.FirstOrDefault(x => x.Language.Code == languageCode)
                        .Title,
                    RegionName = ttr
                        .Region.Translations.FirstOrDefault(x => x.Language.Code == languageCode)
                        .Title,
                    CityName = ttr
                        .City.Translations.FirstOrDefault(x => x.Language.Code == languageCode)
                        .Title,
                    DistrictName = ttr
                        .District.Translations.FirstOrDefault(x => x.Language.Code == languageCode)
                        .Title,
                    VehicleName = ttr.Vehicle.VehicleName,
                    DriverName = ttr.Driver.NameSurname
                })
                .ToList(),
        });
        var response = new TourQueryResponse { Tours = dtos };
        return response;
    }
}
