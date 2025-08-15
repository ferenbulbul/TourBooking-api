using Microsoft.AspNetCore.HttpOverrides;
using TourBooking.API.Exceptions;
using TourBooking.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Cloud Run $PORT'u dinle
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// --- Eğer Cloud Run env'leri geldiyse, connection string'i override et ---
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPass = Environment.GetEnvironmentVariable("DB_PASS");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var instance = Environment.GetEnvironmentVariable("INSTANCE_CONNECTION_NAME"); // project:region:instance

if (!string.IsNullOrWhiteSpace(dbUser) &&
    !string.IsNullOrWhiteSpace(dbPass) &&
    !string.IsNullOrWhiteSpace(dbName) &&
    !string.IsNullOrWhiteSpace(instance))
{
    // Cloud Run, bu soketi otomatik mount eder (Cloud Run dokümanı).
    var socketPath = $"/cloudsql/{instance}";

    // MySqlConnector: Server'a soket yolunu verirsen Unix socket kullanır (doküman).
    var connStr =
        $"Server={socketPath};" +           // alternatif: $"UnixSocket={socketPath};"
        $"ConnectionProtocol=Unix;" +       // açıkça belirtmek istersen (opsiyonel)
        $"User Id={dbUser};" +
        $"Password={dbPass};" +
        $"Database={dbName};";

    // Senin AddPersistenceServices(configuration) zaten ConnectionStrings:Default okuyor
    configuration["ConnectionStrings:Default"] = connStr;
}

// --- senin mevcut servis kayıtların ---
builder.Services.AddCors(o => o.AddPolicy("AllowMobileApp", p =>
    p.WithOrigins("http://localhost:5173","http://localhost:60729")
     .AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

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

// Cloud Run reverse proxy uyumu
app.UseForwardedHeaders(new ForwardedHeadersOptions {
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

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

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
app.MapControllers();

// (Opsiyonel) ilk açılışta migration
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<YourDbContext>();
//     db.Database.Migrate();
// }

app.Run();
