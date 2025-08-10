using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class MobileSearchGuideQueryHandler
    : IRequestHandler<MobileSearchGuideQuery, MobileSearchGuideQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public MobileSearchGuideQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MobileSearchGuideQueryResponse> Handle(
        MobileSearchGuideQuery request,
        CancellationToken cancellationToken
    )
    {
        var guides = await _unitOfWork.MobileSearchGuides(request);

        if (guides == null || !guides.Any())
        {
            return new MobileSearchGuideQueryResponse();
        }

        var response = new MobileSearchGuideQueryResponse { Guides = guides };
        return response;
    }
}
