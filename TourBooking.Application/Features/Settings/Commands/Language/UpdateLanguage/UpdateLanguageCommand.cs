using MediatR;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class UpdateLanguageCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
