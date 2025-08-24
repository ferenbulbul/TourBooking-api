using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features
{
    public class UpsertDriverCommandHandler : IRequestHandler<UpsertDriverCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        IStringLocalizer<UpsertDriverCommandHandler> _localizer;


        public UpsertDriverCommandHandler(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IStringLocalizer<UpsertDriverCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task Handle(UpsertDriverCommand request, CancellationToken cancellationToken)
        {
            try
            {


                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.Driver(request.Id.Value);
                    var existingAppUser = await _userManager.FindByEmailAsync(request.Email);
                    if (existing != null)
                    {
                        existing.DateOfBirth = request.DateOfBirth;
                        existing.DriversLicenceDocument = request.DriversLicenceDocument;
                        existing.ExperienceYears = request.ExperienceYears;
                        existing.IdentityNumber = request.IdentityNumber;
                        existing.IsActive = request.IsActive;
                        existing.LanguagesSpoken = request.LanguagesSpoken;
                        existing.NameSurname = request.NameSurname;
                        existing.ProfilePhoto = request.ProfilePhoto;
                        existing.ServiceCities = request.ServiceCities;
                        existing.SrcDocument = request.SrcDocument;
                        existing.PsikoDocument = request.PsikoDocument;

                        await _unitOfWork.GetRepository<DriverEntity>().UpdateAsync(existing);

                    }
                    if (existingAppUser != null)
                    {
                        existingAppUser.PhoneNumber = request.PhoneNumber;
                        await _userManager.UpdateAsync(existingAppUser);
                    }
                }
                else
                {
                    var emailExists = await _userManager.FindByEmailAsync(request.Email);
                    if (emailExists != null)
                    {
                        throw new BusinessRuleValidationException(_localizer["EmailAlreadyInUse"]);
                    }
                    var newUser = new AppUser
                    {
                        FirstName = request.NameSurname,
                        LastName = request.NameSurname,
                        Email = request.Email,
                        UserName = request.Email,
                        EmailConfirmed = true,
                        UserType = UserType.Driver,
                        PhoneNumber = request.PhoneNumber
                    };
                    var result = await _userManager.CreateAsync(newUser, "Driver.123");
                    if (!result.Succeeded)
                    {
                        var errors = result.Errors.Select(e => e.Description).ToList();
                        throw new ValidationException(errors);
                    }


                    var driver = new DriverEntity
                    {
                        Id = newUser.Id,
                        DateOfBirth = request.DateOfBirth,
                        DriversLicenceDocument = request.DriversLicenceDocument,
                        ExperienceYears = request.ExperienceYears,
                        IdentityNumber = request.IdentityNumber,
                        IsActive = true,
                        LanguagesSpoken = request.LanguagesSpoken,
                        NameSurname = request.NameSurname,
                        ProfilePhoto = request.ProfilePhoto,
                        ServiceCities = request.ServiceCities,
                        SrcDocument = request.SrcDocument,
                        AgencyId = request.AgencyId,
                        PsikoDocument = request.PsikoDocument,

                    };
                    await _unitOfWork.GetRepository<DriverEntity>().AddAsync(driver);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
