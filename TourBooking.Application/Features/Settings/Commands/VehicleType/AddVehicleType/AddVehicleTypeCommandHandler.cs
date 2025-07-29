using System;
using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class AddVehicleTypeCommandHandler : IRequestHandler<AddVehicleTypeCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddVehicleTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddVehicleTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.VehicleType(request.Id.Value);

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
                                    new VehicleTypeTranslation
                                    {
                                        Title = newTr.Title,
                                        Description = newTr.Description,
                                        LanguageId = newTr.LanguageId
                                    }
                                );
                            }
                        }

                        await _unitOfWork.GetRepository<VehicleType>().UpdateAsync(existing);
                    }
                }
                else
                {
                    var vehicleType = new VehicleType
                    {
                        Translations = request
                            .Translations.Select(t => new VehicleTypeTranslation
                            {
                                Title = t.Title,
                                Description = t.Description,
                                LanguageId = t.LanguageId,
                            })
                            .ToList()
                    };
                    await _unitOfWork.GetRepository<VehicleType>().AddAsync(vehicleType);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
