using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings
{
    public class CreateBlockCommandHandler : IRequestHandler<CreateBlockCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBlockCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateBlockCommand request, CancellationToken cancellationToken)
        {
            if (request.End < request.Start)
                throw new ArgumentException("End must be >= Start");
            await _unitOfWork.CreateGuideBlock(request);
        }
    }
}
