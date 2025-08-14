using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class FetchVehicleEventsQueryHandler
        : IRequestHandler<FetchVehicleEventsQuery, FetchVehicleEventsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FetchVehicleEventsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FetchVehicleEventsQueryResponse> Handle(
            FetchVehicleEventsQuery request,
            CancellationToken cancellationToken
        )
        {
            var events = await _unitOfWork.VehicleEvents(request);

            if (events == null || !events.Any())
            {
                return new FetchVehicleEventsQueryResponse();
            }
            return new FetchVehicleEventsQueryResponse{ Events = events};
        }
    }
}
