using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features;

public class MobileDetailedSearchQueryHandler
    : IRequestHandler<MobileDetailedSearchQuery, MobileDetailedSearchQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public MobileDetailedSearchQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MobileDetailedSearchQueryResponse> Handle(
        MobileDetailedSearchQuery request,
        CancellationToken cancellationToken
    )
    {
        var culture = CultureInfo.CurrentUICulture.Name;

        // Kalkış Noktasına Göre
        if (request.Type == (int)MobileSearchType.TourPoint)
        {
            var points = await _unitOfWork.MobileTourPointsByLocation(
                request.CityId,
                request.DistrictId,
                culture
            );

            if (points == null || !points.Any())
            {
                return new MobileDetailedSearchQueryResponse();
            }

            return new MobileDetailedSearchQueryResponse { TourPoints = points };
        }
        else if (request.Type == (int)MobileSearchType.DeparturePoint)
        {
            var points = await _unitOfWork.MobileTourPointsByDeparture(
                request.CityId,
                request.DistrictId,
                culture
            );

            if (points == null || !points.Any())
            {
                return new MobileDetailedSearchQueryResponse();
            }
            return new MobileDetailedSearchQueryResponse { TourPoints = points };
        }
        return new MobileDetailedSearchQueryResponse();
    }
}
