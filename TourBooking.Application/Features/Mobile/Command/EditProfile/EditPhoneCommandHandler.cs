using System.Globalization;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Features.Mobile.Query.TourPointDetails;
using TourBooking.Application.Features.Settings;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features;

public class EditPhoneCommandHandler
    : IRequestHandler<EditPhoneCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;

    public EditPhoneCommandHandler(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task Handle(
        EditPhoneCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingUser = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (existingUser == null)
        {
            throw new BusinessRuleValidationException("");
        }
        existingUser.PhoneNumberConfirmed = false;
        existingUser.PhoneNumber = request.PhoneNumber;
        await _userManager.UpdateAsync(existingUser);

    }
}
