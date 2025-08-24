using System;
using MediatR;
using TourBooking.Application.Features.Admin.Query.AgenciesToConfirm;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features.Admin
{
    public class AdminManagementUserCommand : IRequest
    {

        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public UserType UserType { get; set; }
    }
}
