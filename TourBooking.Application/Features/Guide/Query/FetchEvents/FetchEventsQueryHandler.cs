using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class FetchEventsQueryHandler
        : IRequestHandler<FetchEventsQuery, FetchEventsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FetchEventsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FetchEventsQueryResponse> Handle(
            FetchEventsQuery request,
            CancellationToken cancellationToken
        )
        {
            var events = await _unitOfWork.GuideEvents(request);

            if (events == null || !events.Any())
            {
                return new FetchEventsQueryResponse();
            }
            return new FetchEventsQueryResponse{ Events = events};
        }
    }
}
