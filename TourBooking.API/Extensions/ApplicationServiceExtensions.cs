using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using TourBooking.Application.Common.Behaviors;

namespace TourBooking.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var appAssembly = Assembly.Load("TourBooking.Application");

            services.AddMediatR(cfg => 
                cfg.RegisterServicesFromAssembly(appAssembly)
            );
              services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(appAssembly));
              services.AddValidatorsFromAssembly(appAssembly);
              services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}