using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features;

public class DriverQueryHandler : IRequestHandler<DriverQuery, DriverQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DriverQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DriverQueryResponse> Handle(
        DriverQuery request,
        CancellationToken cancellationToken
    )
    {
        var drivers = await _unitOfWork.Drivers();

        if (drivers == null || !drivers.Any())
        {
            throw new NotFoundException("Tur noktası bulunamadı.");
        }
        var dtos = drivers.Select(tt => new DriverDto
        {
            Id = tt.Id,
            DateOfBirth = tt.DateOfBirth,
            DriversLicenceDocument = tt.DriversLicenceDocument,
            ExperienceYears = tt.ExperienceYears,
            IdentityNumber = tt.IdentityNumber,
            IsActive = tt.IsActive,
            LanguagesSpoken = tt.LanguagesSpoken,
            NameSurname = tt.NameSurname,
            ProfilePhoto = tt.ProfilePhoto,
            ServiceCities = tt.ServiceCities,
            SrcDocument = tt.SrcDocument
        });
        var response = new DriverQueryResponse { Drivers = dtos };
        return response;
    }
}
