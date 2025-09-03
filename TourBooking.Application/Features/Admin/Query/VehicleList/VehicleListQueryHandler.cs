using System;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TourBooking.Application.DTOs.Admin;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features.Admin.Query
{
    public class VehicleListQueryHandler
        : IRequestHandler<VehicleListQuery, VehicleListQueryResponse>
    {
        private readonly IUnitOfWork _uow;

        public VehicleListQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<VehicleListQueryResponse> Handle(
            VehicleListQuery request,
            CancellationToken cancellationToken
        )
        {
            var vehicles = await _uow.GetVehiclesAsync();
            return new VehicleListQueryResponse { Vehicles = vehicles };
        }
    }
}
