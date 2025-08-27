using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Comman;
using TourBooking.Domain.Entities;
using TourBooking.Infrastructure.Repositories;
using TourBooking.Infrastructure.Services;

namespace TourBooking.API.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));


            var netgsmOptions = new NetgsmOptions
            {
                Username = Environment.GetEnvironmentVariable("NETGSM_USERNAME"),
                Password = Environment.GetEnvironmentVariable("NETGSM_PASSWORD"),
                MsgHeader = Environment.GetEnvironmentVariable("NETGSM_MSGHEADER"),
            };
            services.AddSingleton(netgsmOptions);

            //services.Configure<NetgsmOptions>(configuration.GetSection("Netgsm"));

            services.AddHttpClient<INetgsmSmsService, NetgsmSmsService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailVerificationCodeRepository, EmailVerificationCodeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}