using Microsoft.EntityFrameworkCore;
using TourBooking.API.Exceptions; // ExceptionHandlingMiddleware için
using TourBooking.API.Extensions;
using TourBooking.Infrastructure.Context; // Oluşturduğumuz tüm extension metotları için

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowMobileApp",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:5173", "http://localhost:60729") // exact origin(s), no wildcard
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var supportedCultures = new[] { "tr", "en" };

var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("tr")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(configuration);
builder.Services.AddIdentityServices(configuration);
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddSwaggerServices();

var app = builder.Build();
app.UseRequestLocalization(localizationOptions);
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    
    // Veritabanı migration'larını uygulama
    context.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowMobileApp");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();
