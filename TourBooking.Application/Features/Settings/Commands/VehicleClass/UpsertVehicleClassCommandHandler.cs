using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class UpsertVehicleClassCommandHandler : IRequestHandler<UpsertVehicleClassCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertVehicleClassCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            UpsertVehicleClassCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.VehicleClass(request.Id.Value);

                    if (existing != null)
                    {
                        foreach (var newTr in request.Translations)
                        {
                            var existingTr = existing.Translations.FirstOrDefault(t =>
                                t.LanguageId == newTr.LanguageId
                            );

                            if (existingTr != null)
                            {
                                existingTr.Title = newTr.Title;
                                existingTr.Description = newTr.Description;
                            }
                            else
                            {
                                existing.Translations.Add(
                                    new VehicleClassTranslation
                                    {
                                        Title = newTr.Title,
                                        Description = newTr.Description,
                                        LanguageId = newTr.LanguageId
                                    }
                                );
                            }
                        }

                        await _unitOfWork.GetRepository<VehicleClassEntity>().UpdateAsync(existing);
                    }
                }
                else
                {
                    var vehicleClass = new VehicleClassEntity
                    {
                        Translations = request
                            .Translations.Select(t => new VehicleClassTranslation
                            {
                                Title = t.Title,
                                Description = t.Description,
                                LanguageId = t.LanguageId,
                            })
                            .ToList()
                    };
                    await _unitOfWork.GetRepository<VehicleClassEntity>().AddAsync(vehicleClass);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
