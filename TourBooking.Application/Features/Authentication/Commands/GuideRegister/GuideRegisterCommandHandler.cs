using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features.Authentication.Commands.Register
{
    public class GuideRegisterCommandHandler
        : IRequestHandler<GuideRegisterCommand, GuideRegisterCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        IStringLocalizer<GuideRegisterCommandHandler> _localizer;

        public GuideRegisterCommandHandler(
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork,
            IStringLocalizer<GuideRegisterCommandHandler> localizer
        )
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<GuideRegisterCommandResponse> Handle(
            GuideRegisterCommand request,
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
                UserType = UserType.Guide,
                PhoneNumber = request.PhoneNumber
            };
            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException(errors);
            }
            var newGuideUser = new GuideUserEntity
            {
                Id = newUser.Id,
                PhoneNumber = request.PhoneNumber,
                CreatedDate = DateTime.Now,
                Email = request.Email,
                IsConfirmed = false,
                IsDeleted = false,
                LicenseNumber = request.LicenseNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            await _unitOfWork.GetRepository<GuideUserEntity>().AddAsync(newGuideUser);
            return new GuideRegisterCommandResponse();
        }
    }
}
