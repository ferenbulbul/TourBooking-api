using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class MobileHighlightedTourPointQueryHandler
    : IRequestHandler<MobileHighlightedTourPointQuery, MobileHighlightedTourPointQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public MobileHighlightedTourPointQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MobileHighlightedTourPointQueryResponse> Handle(
        MobileHighlightedTourPointQuery request,
        CancellationToken cancellationToken
    )
    {
        var points = await _unitOfWork.HighlightedTourPoints();
        if (points == null || !points.Any())
        {
            throw new NotFoundException("Tur noktası bulunamadı.");
        }
        var response = new MobileHighlightedTourPointQueryResponse { TourPoints = points };
        return response;
    }
}
