using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        IStringLocalizer<RegisterCommandHandler> _localizer;


        public RegisterCommandHandler(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, ITokenService tokenService, IStringLocalizer<RegisterCommandHandler> localizer)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _localizer = localizer;
        }

        public async Task<RegisterCommandResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _userManager.FindByEmailAsync(request.Email);
            if (emailExists != null)
            {
                throw new BusinessRuleValidationException(_localizer["EmailAlreadyInUse"]);
            }

            var phoneExists = await _userManager.Users
                .AnyAsync(u => u.PhoneNumber == request.PhoneNumber);

            if (phoneExists)
            {
                throw new BusinessRuleValidationException(_localizer["PhoneNumberAlreadyInUse"]);
            }


            var newUser = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                EmailConfirmed = false,
                UserType = UserType.Customer,
                PhoneNumber = request.PhoneNumber
            };
            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new Expactions.ValidationException(errors);
            }
            var newCustomerUser = new CustomerUser
            {
                Id = newUser.Id,
                PhoneNumber = request.PhoneNumber,
                BirthDate = request.BirthDate
            };
            var user = await _userManager.FindByEmailAsync(request.Email);
            var token = await _tokenService.CreateTokenAsync(user);
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpireDate = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            await _unitOfWork.GetRepository<CustomerUser>().AddAsync(newCustomerUser);
            var response = new RegisterCommandResponse
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                UserFullName = $"{user.FirstName} {user.LastName}",
                Role = user.UserType.ToString(),
                EmailConfirmed = user.EmailConfirmed,
            };
            return response;
        }
    }
}