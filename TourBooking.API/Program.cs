using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

using TourBooking.API.Exceptions;  // ExceptionHandlingMiddleware
using TourBooking.API.Extensions;
using TourBooking.Infrastructure.Context;  // Add*Services extension'ların (Application/Persistence/Identity/Infrastructure/Swagger)

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Cloud Run: $PORT'u dinle
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");


var dbUser   = Environment.GetEnvironmentVariable("DB_USER");
var dbPass   = Environment.GetEnvironmentVariable("DB_PASS");
var dbName   = Environment.GetEnvironmentVariable("DB_NAME");
var instance = Environment.GetEnvironmentVariable("INSTANCE_CONNECTION_NAME"); // apiv1-469110:europe-west2:tourrentdb
var dbHost   = Environment.GetEnvironmentVariable("DB_HOST"); // boş bırak (socket kullanıyoruz)

string connStr;
if (!string.IsNullOrWhiteSpace(dbHost))
{
    // TCP fallback (Private IP + VPC connector kullanırsan)
    connStr = $"Server={dbHost};Port=3306;User Id={dbUser};Password={dbPass};Database={dbName};SslMode=None;";
}
else
{
    // 🔑 Unix socket yolu — Cloud SQL connection mount edildiğinde çalışır
    connStr =
        $"Server=localhost;" +
        $"User Id={dbUser};Password={dbPass};Database={dbName};" +
        $"UnixSocket=/cloudsql/{instance};SslMode=None;";
}

builder.Configuration["ConnectionStrings:Default"] = connStr;

// ===== CORS / Controllers / Localization =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMobileApp", p =>
        p.WithOrigins("http://localhost:5173", "http://localhost:60729")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var supportedCultures = new[] { "tr", "en" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("tr")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

// ===== Senin extension'ların =====
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(configuration);
builder.Services.AddIdentityServices(configuration);
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddSwaggerServices();

var app = builder.Build();

// ===== Cloud Run reverse proxy başlıkları =====
var fwd = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    RequireHeaderSymmetry = false,
    ForwardLimit = null
};
fwd.KnownNetworks.Clear();
fwd.KnownProxies.Clear();
app.UseForwardedHeaders(fwd);

// ===== Localization =====
app.UseRequestLocalization(localizationOptions);

// ===== Swagger (sadece Development) =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Cloud Run TLS'yi dışta sonlandırır; aşağıdaki redirect X-Forwarded-Proto ile uyumlu çalışır.
app.UseHttpsRedirection();

// ===== Pipeline =====
app.UseCors("AllowMobileApp");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// ===== Basit test endpoint'leri =====
app.MapGet("/", () => Results.Ok(new { status = "ok" }));
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.MapGet("/cloudsql-debug", () =>
{
    var instance = Environment.GetEnvironmentVariable("INSTANCE_CONNECTION_NAME");
    var dbUser   = Environment.GetEnvironmentVariable("DB_USER");
    var dbPass   = Environment.GetEnvironmentVariable("DB_PASS");
    var dbName   = Environment.GetEnvironmentVariable("DB_NAME");

    var basePath = "/cloudsql";
    var dirExists = Directory.Exists(basePath);
    string[] entries = Array.Empty<string>();
    if (dirExists)
    {
        try { entries = Directory.GetFileSystemEntries(basePath); } catch {}
    }

    return Results.Ok(new
    {
        env = new {
            INSTANCE_CONNECTION_NAME = string.IsNullOrWhiteSpace(instance) ? null : instance,
            DB_USER_set   = !string.IsNullOrEmpty(dbUser),
            DB_PASS_set   = !string.IsNullOrEmpty(dbPass),
            DB_NAME_value = dbName
        },
        cloudsql = new {
            basePath,
            baseDirExists = dirExists,
            entries // burada genelde "project:region:instance" görmelisin
        },
        expectedSocket = instance is null ? null : $"/cloudsql/{instance}"
    });
});

// Hata mesajını daha detaylı dönen ping
app.MapGet("/db-ping2", async () =>
{
    try
    {
        var cs = builder.Configuration.GetConnectionString("Default");
        await using var conn = new MySqlConnection(cs);
        await conn.OpenAsync();

        await using var cmd = new MySqlCommand("SELECT DATABASE(), USER(), VERSION()", conn);
        await using var r = await cmd.ExecuteReaderAsync();
        string? database=null, user=null, version=null;
        if (await r.ReadAsync()) {
            database = r.IsDBNull(0) ? null : r.GetString(0);
            user     = r.IsDBNull(1) ? null : r.GetString(1);
            version  = r.IsDBNull(2) ? null : r.GetString(2);
        }
        return Results.Ok(new { ok=true, database, user, version });
    }
    catch (Exception ex)
    {
        return Results.Problem(new {
            message = "db-ping failed",
            type = ex.GetType().FullName,
            ex.Message,
            inner = ex.InnerException?.Message
        }.ToString());
    }
});
app.MapGet("/db-ping", async () =>
{
    try
    {
        var cs = builder.Configuration.GetConnectionString("Default");
        await using var conn = new MySqlConnection(cs);
        await conn.OpenAsync();

        await using var cmd = new MySqlCommand("SELECT DATABASE(), USER(), VERSION()", conn);
        await using var r = await cmd.ExecuteReaderAsync();

        string? database = null, user = null, version = null;
        if (await r.ReadAsync())
        {
            database = r.IsDBNull(0) ? null : r.GetString(0);
            user     = r.IsDBNull(1) ? null : r.GetString(1);
            version  = r.IsDBNull(2) ? null : r.GetString(2);
        }

        return Results.Ok(new { ok = true, database, user, version });
    }
    catch (Exception ex)
    {
        return Results.Problem("db-ping failed: " + ex.Message);
    }
});

// EF Core katmanı üzerinden de kontrol etmek istersen type adını verip aç:
 app.MapGet("/db-health", async (AppDbContext db) =>
 {
     try { return Results.Ok(new { canConnect = await db.Database.CanConnectAsync() }); }
    catch (Exception ex) { return Results.Problem("db-health failed: " + ex.Message); }
 });

// Controller'lar
app.MapControllers();

// ===== (İsteğe bağlı) İlk açılışta migration =====
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<YourDbContext>(); // kendi DbContext adın
//     db.Database.Migrate();
// }

app.Run();
