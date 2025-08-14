using Microsoft.EntityFrameworkCore;
using TourBooking.Infrastructure.Context;

namespace TourBooking.API.Extensions
{
    public static class PersistenceServiceExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // services.AddDbContext<AppDbContext>(options =>
            // {
            //     var connectionString = configuration.GetConnectionString("Default");
            //     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            // });

            services.AddDbContext<AppDbContext>(options =>
                 options.UseMySql(
                 configuration.GetConnectionString("DefaultConnection"),
                  ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))
           )
);
            return services;
        }
    }
}