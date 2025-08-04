using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Interfaces.Repositories;

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
        var drivers = await _unitOfWork.DriversForAgency(request.AgencyId);

        if (drivers == null || !drivers.Any())
        {
            return new DriverQueryResponse();
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
