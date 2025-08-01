using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features
{
    public class UpsertVehicleAvailableCommandHandler
        : IRequestHandler<UpsertVehicleAvailableCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertVehicleAvailableCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            UpsertVehicleAvailableCommand request,
            CancellationToken cancellationToken
        ) { }
    }
}
