using System;
using MediatR;

namespace TourBooking.Application.Features
{
    public class CustomerProfileQuery : IRequest<CustomerProfileQueryResponse>
    {
        public Guid UserId { get; set; }
    }
}
