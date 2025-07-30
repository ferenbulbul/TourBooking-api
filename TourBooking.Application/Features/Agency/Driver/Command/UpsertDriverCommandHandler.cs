using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features
{
    public class UpsertDriverCommandHandler : IRequestHandler<UpsertDriverCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertDriverCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpsertDriverCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.Driver(request.Id.Value);

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

                        await _unitOfWork.GetRepository<DriverEntity>().UpdateAsync(existing);
                    }
                }
                else
                {
                    var driver = new DriverEntity
                    {
                        DateOfBirth = request.DateOfBirth,
                        DriversLicenceDocument = request.DriversLicenceDocument,
                        ExperienceYears = request.ExperienceYears,
                        IdentityNumber = request.IdentityNumber,
                        IsActive = request.IsActive,
                        LanguagesSpoken = request.LanguagesSpoken,
                        NameSurname = request.NameSurname,
                        ProfilePhoto = request.ProfilePhoto,
                        ServiceCities = request.ServiceCities,
                        SrcDocument = request.SrcDocument
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
