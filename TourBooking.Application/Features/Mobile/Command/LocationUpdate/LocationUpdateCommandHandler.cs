// Application/Features/Location/Commands/UpsertUserLocationCommandHandler.cs
using MediatR;
using TourBooking.Application.Features;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

public class UpsertUserLocationCommandHandler : IRequestHandler<LocationUpdateCommand>
{
    private readonly IUnitOfWork _uow;

    public UpsertUserLocationCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(LocationUpdateCommand request, CancellationToken ct)
    {
        // (Opsiyonel) hızlı validasyon
        if (request.Latitude is < -90 or > 90 || request.Longitude is < -180 or > 180)
            throw new ArgumentOutOfRangeException(nameof(request), "Geçersiz koordinat.");

        try
        {
            switch (request.Role)
            {
                case UserType.Driver:
                {
                    var repo = _uow.GetRepository<DriverLocationEntity>();

                    // Shared PK = DriverId
                    var existing = await repo.GetByIdAsync(request.UserId);
                    if (existing is not null)
                    {
                        existing.Latitude  = request.Latitude;
                        existing.Longitude = request.Longitude;
                        existing.UpdatedAt = DateTime.UtcNow;
                        await repo.UpdateAsync(existing);
                    }
                    else
                    {
                        var entity = new DriverLocationEntity
                        {
                            Id  = request.UserId,
                            Latitude  = request.Latitude,
                            Longitude = request.Longitude,
                            UpdatedAt = DateTime.UtcNow
                        };
                        await repo.AddAsync(entity);
                    }
                    break;
                }

                case UserType.Customer:
                {
                    var repo = _uow.GetRepository<CustomerLocationEntity>();

                    // Shared PK = CustomerId
                    var existing = await repo.GetByIdAsync(request.UserId);
                    if (existing is not null)
                    {
                        existing.Latitude  = request.Latitude;
                        existing.Longitude = request.Longitude;
                        existing.UpdatedAt = DateTime.UtcNow;
                        await repo.UpdateAsync(existing);
                    }
                    else
                    {
                        var entity = new CustomerLocationEntity
                        {
                            Id = request.UserId,
                            Latitude   = request.Latitude,
                            Longitude  = request.Longitude,
                            UpdatedAt  = DateTime.UtcNow
                        };
                        await repo.AddAsync(entity);
                    }
                    break;
                }

                default:
                    throw new NotSupportedException($"Desteklenmeyen rol: {request.Role}");
            }

        }
        catch
        {
            throw;
        }
    }
}
