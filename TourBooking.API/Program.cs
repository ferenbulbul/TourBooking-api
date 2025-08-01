using TourBooking.API.Exceptions; // ExceptionHandlingMiddleware için
using TourBooking.API.Extensions; // Oluşturduğumuz tüm extension metotları için

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration; 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMobileApp",
        builder =>
        {
            builder
                .AllowAnyOrigin() 
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
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


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowMobileApp");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();


app.Run();