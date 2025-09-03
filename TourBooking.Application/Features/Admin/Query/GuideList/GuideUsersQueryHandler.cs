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
    public class GuideUsersQueryHandler
        : IRequestHandler<GuideUsersQuery, GuideUsersQueryResponse>
    {
        private readonly IUnitOfWork _uow;

        public GuideUsersQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<GuideUsersQueryResponse> Handle(
            GuideUsersQuery request,
            CancellationToken cancellationToken
        )
        {
            var guides = await _uow.GetGuideListAsnyc();
            return new GuideUsersQueryResponse { Guides = guides };
        }
    }
}
