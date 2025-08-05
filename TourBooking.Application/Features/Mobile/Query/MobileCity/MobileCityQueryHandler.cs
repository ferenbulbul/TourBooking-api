using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class MobileCityQueryHandler : IRequestHandler<MobileCityQuery, MobileCityQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public MobileCityQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MobileCityQueryResponse> Handle(
        MobileCityQuery request,
        CancellationToken cancellationToken
    )
    {
        var culture = CultureInfo.CurrentUICulture.Name;

        var cities = await _unitOfWork.CitiesForMobile(culture);

        if (cities == null || !cities.Any())
        {
            return new MobileCityQueryResponse();
        }

        var response = new MobileCityQueryResponse { CityList = cities };
        return response;
    }
}
