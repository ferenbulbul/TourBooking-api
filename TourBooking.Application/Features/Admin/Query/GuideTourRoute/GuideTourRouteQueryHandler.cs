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
    public class GuideTourRouteQueryHandler
        : IRequestHandler<GuideTourRouteQuery, GuideTourRouteQueryResponse>
    {
        private readonly IUnitOfWork _uow;

        public GuideTourRouteQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<GuideTourRouteQueryResponse> Handle(
            GuideTourRouteQuery request,
            CancellationToken cancellationToken
        )
        {
            var routes = await _uow.GetGuideToursAsnyc();
            return new GuideTourRouteQueryResponse { Routes = routes };
        }
    }
}
