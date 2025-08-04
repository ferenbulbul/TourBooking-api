using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class AddVehicleBrandCommandHandler : IRequestHandler<AddVehicleBrandCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddVehicleBrandCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            AddVehicleBrandCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.VehicleBrand(request.Id.Value);

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
                                    new VehicleBrandTranslation
                                    {
                                        Title = newTr.Title,
                                        Description = newTr.Description,
                                        LanguageId = newTr.LanguageId
                                    }
                                );
                            }
                        }

                        await _unitOfWork.GetRepository<VehicleBrand>().UpdateAsync(existing);
                    }
                }
                else
                {
                    var vehicleBrand = new VehicleBrand
                    {
                        Translations = request
                            .Translations.Select(t => new VehicleBrandTranslation
                            {
                                Title = t.Title,
                                Description = t.Description,
                                LanguageId = t.LanguageId,
                            })
                            .ToList()
                    };
                    await _unitOfWork.GetRepository<VehicleBrand>().AddAsync(vehicleBrand);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
