using System;
using MediatR;
using TourBooking.Application.Features.Admin.Query.AgenciesToConfirm;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features.Admin
{
    public class DeleteAdminUserCommand : IRequest
    {
        public string UserId { get; set; }
    }
}
