using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features
{
    public class UpsertTourCommandHandler : IRequestHandler<UpsertTourCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertTourCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpsertTourCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.Tour(request.Id.Value);

                    if (existing != null)
                    {
                        existing.TourPointId = request.TourPointId;
                        if (request.Pricing != null && request.Pricing.Any())
                        {
                            foreach (var newPricing in request.Pricing)
                            {
                                var existingPricing = existing.TourRoutePriceEntity.FirstOrDefault(p =>
                                    p.Id == newPricing.Id
                                );

                                if (existingPricing != null)
                                {
                                    existingPricing.CountryId = newPricing.CountryId;
                                    existingPricing.CityId = newPricing.CityId;
                                    existingPricing.DistrictId = newPricing.DistrictId;
                                    existingPricing.RegionId = newPricing.RegionId;
                                    existingPricing.VehicleId = newPricing.VehicleId;
                                    existingPricing.DriverId = newPricing.DriverId;
                                    existingPricing.UpdatedDate = DateTime.UtcNow;
                                    existingPricing.Price = newPricing.Price;
                                    // Diğer alanlar...
                                }
                                else
                                {
                                    // Yeni ekle
                                    existing.TourRoutePriceEntity.Add(
                                        new TourRoutePriceEntity
                                        {
                                            Id = Guid.NewGuid(),
                                            CountryId = newPricing.CountryId,
                                            CityId = newPricing.CityId,
                                            DistrictId = newPricing.DistrictId,
                                            RegionId = newPricing.RegionId,
                                            VehicleId = newPricing.VehicleId,
                                            DriverId = newPricing.DriverId,
                                            CreatedDate = DateTime.UtcNow,
                                            Price = newPricing.Price,
                                            // Diğer alanlar...
                                        }
                                    );
                                }
                            }

                            // Silinecekleri bul ve kaldır (gerekirse)
                            var newPricingIds = request
                                .Pricing.Where(x => x.Id != Guid.Empty)
                                .Select(x => x.Id)
                                .ToList();

                            var toRemove = existing
                                .TourRoutePriceEntity.Where(pe =>
                                    pe.Id != Guid.Empty && !newPricingIds.Contains(pe.Id)
                                )
                                .ToList();

                            foreach (var item in toRemove)
                                existing.TourRoutePriceEntity.Remove(item);
                        }
                    }
                }
                else
                {
                    var tourPoint = new TourEntity
                    {
                        TourPointId = request.TourPointId,
                        AgencyId = request.AgencyId,
                        TourRoutePriceEntity = request
                            .Pricing.Select(t => new TourRoutePriceEntity
                            {
                                CityId = t.CityId,
                                CountryId = t.CountryId,
                                DistrictId = t.DistrictId,
                                RegionId = t.RegionId,
                                VehicleId = t.VehicleId,
                                DriverId = t.DriverId,
                                Price = t.Price
                            })
                            .ToList()
                    };
                    await _unitOfWork.GetRepository<TourEntity>().AddAsync(tourPoint);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
