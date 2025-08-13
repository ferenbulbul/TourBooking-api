using System;
using MediatR;

namespace TourBooking.Application.Features.Authentication.Queries.IsApproved
{
    public class IsApprovedQuery : IRequest<IsApprovedQueryResponse>
    {
        public Guid UserId { get; set; }
        public string? Role { get; set; }
    }
}
