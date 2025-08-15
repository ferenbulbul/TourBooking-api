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
                {
                    var cs = configuration.GetConnectionString("Default"); // <-- ayni anahtar
                    options.UseMySql(cs, ServerVersion.AutoDetect(cs),
                        b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
                });
            return services;
        }
    }
}