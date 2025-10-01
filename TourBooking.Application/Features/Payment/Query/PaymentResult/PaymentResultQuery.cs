using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TourBooking.Application.Features.Payment.Query.PaymentResult
{
    public class PaymentResultQuery : IRequest<PaymentResultQueryResponse>
    {
        public string Token { get; set; } = default!;
    }
}