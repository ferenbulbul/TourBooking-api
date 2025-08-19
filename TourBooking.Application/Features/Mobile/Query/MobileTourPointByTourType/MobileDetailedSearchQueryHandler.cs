using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features;

public class MobileTourPointByTourTypeQueryHandler
    : IRequestHandler<MobileTourPointByTourTypeQuery, MobileTourPointByTourTypeQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public MobileTourPointByTourTypeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MobileTourPointByTourTypeQueryResponse> Handle(
        MobileTourPointByTourTypeQuery request,
        CancellationToken cancellationToken
    )
    {
        var culture = CultureInfo.CurrentUICulture.Name;

        var points = await _unitOfWork.MobileTourPointByTourTypeId(
            request.TourType,culture
        );

        if (points == null || !points.Any())
        {
            return new MobileTourPointByTourTypeQueryResponse();
        }

        return new MobileTourPointByTourTypeQueryResponse { TourPoints = points };

    }
}
