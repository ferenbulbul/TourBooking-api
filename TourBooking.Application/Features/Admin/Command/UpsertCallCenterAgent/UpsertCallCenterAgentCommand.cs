using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class UpsertCallCenterAgentCommand : IRequest
    {
        public Guid? Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Firm { get; set; }
        public bool IsActive { get; set; }
    }
}
