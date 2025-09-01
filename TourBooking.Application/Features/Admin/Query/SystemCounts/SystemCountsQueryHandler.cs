using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class SystemCountsQueryHandler
    : IRequestHandler<SystemCountsQuery, SystemCountsQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public SystemCountsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SystemCountsQueryResponse> Handle(
        SystemCountsQuery request,
        CancellationToken cancellationToken
    )
    {
        var resultList = await _unitOfWork.SystemCounts();
        if (resultList == null)
        {
            return new SystemCountsQueryResponse();
        }
        return new SystemCountsQueryResponse { Counts = resultList };
    }
}
