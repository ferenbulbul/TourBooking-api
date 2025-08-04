using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class AddLanguageCommandHandler : IRequestHandler<AddLanguageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddLanguageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddLanguageCommand request, CancellationToken cancellationToken)
        {
            var language = new LanguageEntity
            {
                Name = request.Name,
                Code = request.Code,
                IsActive = request.IsActive,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
            };
            await _unitOfWork.GetRepository<LanguageEntity>().AddAsync(language);
        }
    }
}
