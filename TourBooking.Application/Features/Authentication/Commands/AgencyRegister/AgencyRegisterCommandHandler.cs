using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features.Authentication.Commands.Register
{
    public class AgencyRegisterCommandHandler
        : IRequestHandler<AgencyRegisterCommand, AgencyRegisterCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        IStringLocalizer<AgencyRegisterCommandHandler> _localizer;

        public AgencyRegisterCommandHandler(
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork,
            IStringLocalizer<AgencyRegisterCommandHandler> localizer
        )
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<AgencyRegisterCommandResponse> Handle(
            AgencyRegisterCommand request,
            CancellationToken cancellationToken
        )
        {
            var emailExists = await _userManager.FindByEmailAsync(request.Email);
            if (emailExists != null)
            {
                throw new BusinessRuleValidationException(_localizer["EmailAlreadyInUse"]);
            }

            var newUser = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                EmailConfirmed = false,
                UserType = UserType.Agency,
                PhoneNumber = request.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new Expactions.ValidationException(errors);
            }
            var newAgencyUser = new AgencyUserEntity
            {
                Id = newUser.Id,
                PhoneNumber = request.PhoneNumber,
                AuthorizedUserFirstName = request.FirstName,
                AuthorizedUserLastName = request.LastName,
                City = request.City,
                CompanyName = request.CompanyName,
                Country = request.Country,
                CreatedDate = DateTime.Now,
                Email = request.Email,
                FullAddress = request.FullAddress,
                IsConfirmed = false,
                IsDeleted = false,
                PhoneNumber2 = request.PhoneNumber2,
                TaxNumber = request.TaxNumber,
                TursabUrl = request.TursabUrl
            };

            await _unitOfWork.GetRepository<AgencyUserEntity>().AddAsync(newAgencyUser);
            return new AgencyRegisterCommandResponse();
        }
    }
}
