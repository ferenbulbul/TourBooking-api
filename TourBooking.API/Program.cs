using TourBooking.API.Exceptions; // ExceptionHandlingMiddleware için
using TourBooking.API.Extensions; // Oluşturduğumuz tüm extension metotları için

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration; 


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Extension metotlarımızı çağırıyoruz
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(configuration);
builder.Services.AddIdentityServices(configuration);
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddSwaggerServices();



var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();