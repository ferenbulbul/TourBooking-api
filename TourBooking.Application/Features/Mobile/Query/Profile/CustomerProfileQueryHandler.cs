using System;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features
{
    public class CustomerProfileQueryHandler : IRequestHandler<CustomerProfileQuery, CustomerProfileQueryResponse>
    {

        private readonly UserManager<AppUser> _manager;

        public CustomerProfileQueryHandler(IUnitOfWork unitOfWork, UserManager<AppUser> manager)
        {
            _manager = manager;
        }

        public async Task<CustomerProfileQueryResponse> Handle(
            CustomerProfileQuery request,
            CancellationToken cancellationToken
        )
        {
            var user=await _manager.FindByIdAsync(request.UserId.ToString());

           var profile= new CustomerProfileQueryResponse
            {
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed=user.PhoneNumberConfirmed
            };
            return profile;

        }
    }
}
