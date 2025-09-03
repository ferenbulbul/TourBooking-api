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
    public class RouteListQueryHandler
        : IRequestHandler<RouteListQuery, RouteListQueryResponse>
    {
        private readonly IUnitOfWork _uow;

        public RouteListQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<RouteListQueryResponse> Handle(
            RouteListQuery request,
            CancellationToken cancellationToken
        )
        {
            var routes = await _uow.GetTourRoute();
            return new RouteListQueryResponse { Routes = routes };
        }
    }
}
