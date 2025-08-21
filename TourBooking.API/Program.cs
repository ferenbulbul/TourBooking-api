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
using TourBooking.Infrastructure.Context;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;  // Add*Services extension'ların (Application/Persistence/Identity/Infrastructure/Swagger)

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Cloud Run: $PORT'u dinle
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// ===== Cloud SQL (Unix socket) için bağlantı dizesini ENV/Secret'lardan üret =====
var dbUser   = Environment.GetEnvironmentVariable("DB_USER");
var dbPass   = Environment.GetEnvironmentVariable("DB_PASS");
var dbName   = Environment.GetEnvironmentVariable("DB_NAME");
var instance = Environment.GetEnvironmentVariable("INSTANCE_CONNECTION_NAME"); // project:region:instance

if (!string.IsNullOrWhiteSpace(dbUser) &&
    !string.IsNullOrWhiteSpace(dbPass) &&
    !string.IsNullOrWhiteSpace(dbName) &&
    !string.IsNullOrWhiteSpace(instance))
{
    var csb = new MySqlConnectionStringBuilder
    {
        // UnixSocket verildiğinde TCP kullanılmaz.
        Server     = $"/cloudsql/{instance}",
        UserID     = dbUser,
        Password   = dbPass,
        Database   = dbName,
        SslMode    = MySqlSslMode.None
    };

    // Persistence katmanının okuduğu anahtarı override ediyoruz.
    configuration["ConnectionStrings:Default"] = csb.ConnectionString;
}

// ===== CORS / Controllers / Localization =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMobileApp", p =>
        p.WithOrigins("http://localhost:5173", "http://localhost:60729","https://app2-595877181314.europe-west1.run.app","https://tourbook-backoffice-272954735037.europe-west2.run.app")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials());
});
var firebaseSettingsPath = builder.Configuration.GetValue<string>("Firebase:ServiceAccountKeyPath");
if (string.IsNullOrEmpty(firebaseSettingsPath))
{
    throw new Exception("Firebase Service Account Key Path is not configured in appsettings.json");
}

// 2. Firebase Admin SDK'yı başlat
// FirebaseApp.Create(new AppOptions()
// {
//     Credential = GoogleCredential.FromFile(firebaseSettingsPath),
// });
var credential = GoogleCredential.FromFile("/secrets/firebase-key.json");

FirebaseApp.Create(new AppOptions()
{
    Credential = credential
});
Console.WriteLine("✅ Firebase Admin SDK successfully initialized.");


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



// ... app.Build() ve pipeline kurulumundan sonra, app.Run'dan ÖNCE:
var migrateFlag = Environment.GetEnvironmentVariable("MIGRATE_ON_STARTUP") == "true";
if (migrateFlag)
{
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        _ = Task.Run(async () =>
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.MigrateAsync();

                var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("Migrate");
                logger.LogInformation("EF Core migration completed.");
            }
            catch (Exception ex)
            {
                try
                {
                    var logger = app.Services.GetRequiredService<ILoggerFactory>()
                        .CreateLogger("Migrate");
                    logger.LogError(ex, "EF Core migration failed.");
                }
                catch { /* logger yoksa sessiz geç */ }
            }
        });
    });
}


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

app.MapGet("/ef-info", async (AppDbContext db) =>
{
    var pending = await db.Database.GetPendingMigrationsAsync();
    var applied = await db.Database.GetAppliedMigrationsAsync();
    return Results.Ok(new { pending, applied });
});

// Manuel migrate tetikle ve hatayı düz metin döndür
app.MapPost("/admin/migrate-now", async (IServiceProvider sp) =>
{
    try
    {
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var before = await db.Database.GetPendingMigrationsAsync();
        await db.Database.MigrateAsync();
        var after = await db.Database.GetPendingMigrationsAsync();
        return Results.Ok(new { migrated = true, pendingBefore = before, pendingAfter = after });
    }
    catch (Exception ex)
    {
        return Results.Text(ex.ToString(), "text/plain"); // middleware'e takılmasın
    }
});
 app.MapGet("/db-health", async (AppDbContext db) =>
 {
     try { return Results.Ok(new { canConnect = await db.Database.CanConnectAsync() }); }
    catch (Exception ex) { return Results.Problem("db-health failed: " + ex.Message); }
 });

app.MapControllers();
app.Run();
