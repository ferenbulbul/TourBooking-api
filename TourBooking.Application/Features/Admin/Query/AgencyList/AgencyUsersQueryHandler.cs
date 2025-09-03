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
    public class AgencyUsersQueryHandler
        : IRequestHandler<AgencyUsersQuery, AgencyUsersQueryResponse>
    {
        private readonly IUnitOfWork _uow;

        public AgencyUsersQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<AgencyUsersQueryResponse> Handle(
            AgencyUsersQuery request,
            CancellationToken cancellationToken
        )
        {
            var users = await _uow.GetAgencyListAsync();
            return new AgencyUsersQueryResponse { Agencies = users };
        }
    }
}
