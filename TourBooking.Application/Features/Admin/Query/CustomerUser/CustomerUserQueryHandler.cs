using System;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TourBooking.Application.DTOs.Admin;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features.Admin.Query
{
    public class CustomerUserQueryHandler
        : IRequestHandler<CustomerUserQuery, CustomerUserQueryResponse>
    {
        private readonly IUnitOfWork _uow;

        public CustomerUserQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<CustomerUserQueryResponse> Handle(
            CustomerUserQuery request,
            CancellationToken cancellationToken
        )
        {
            var users = await _uow.GetCustomerUsersWithAspNetAsync();
            return new CustomerUserQueryResponse { Customers = users };
        }
    }
}
