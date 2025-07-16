using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ValidationException = TourBooking.Application.Expactions.ValidationException;

namespace TourBooking.Application.Common.Behaviors
{
    // Bu, MediatR pipeline'ına takılan bir ara katman gibidir.
    // IRequest olan her şey için çalışır.
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .Select(f => f.ErrorMessage) // Sadece hata mesajlarını al
                .ToList();

            if (failures.Count != 0)
            {
                // Bizim kendi ValidationException'ımızı fırlat!
                // Bu exception'ı, ExceptionHandlingMiddleware'imiz yakalayacak.
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}