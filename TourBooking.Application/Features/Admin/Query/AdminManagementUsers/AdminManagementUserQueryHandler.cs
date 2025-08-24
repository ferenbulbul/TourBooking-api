using System;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TourBooking.Application.DTOs.Admin;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features.Admin.Query.AgenciesToConfirm
{
    public class AdminManagementUserQueryHandler
        : IRequestHandler<AdminManagementUserQuery, AdminManagementUserQueryResponse>
    {
        private readonly UserManager<AppUser> _manager;

        public AdminManagementUserQueryHandler(UserManager<AppUser> manager)
        {
            _manager = manager;
        }

        public async Task<AdminManagementUserQueryResponse> Handle(
            AdminManagementUserQuery request,
            CancellationToken cancellationToken
        )
        {
            var users = await _manager.Users
            .Where(u => u.UserType == UserType.Admin
             || u.UserType == UserType.CallCenterAgent)
                .Select(x => new AdminManagementUserDto(
        x.Id,
        x.FirstName,
        x.LastName,
        x.Email,
        x.PhoneNumber
    ))
    .ToListAsync();


            return new AdminManagementUserQueryResponse
            {
                Users = users
            };
        }
    }
}
