using System;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TourBooking.Application.DTOs.Admin;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features.Admin
{
    public class DeleteAdminUserCommandHandler
        : IRequestHandler<DeleteAdminUserCommand>
    {
        private readonly UserManager<AppUser> _userManager;

        public DeleteAdminUserCommandHandler(UserManager<AppUser> manager)
        {
            _userManager = manager;
        }

        public async Task Handle(
            DeleteAdminUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var existing = await _userManager.FindByIdAsync(request.UserId);
            if (existing == null)
            {
                throw new BusinessRuleValidationException("Silinecek kullanıcı bulunamadı");
            }
            await _userManager.DeleteAsync(existing);
        }
    }
}
