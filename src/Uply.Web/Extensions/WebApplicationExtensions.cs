using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Uply.Domain.Abstractions.Services;
using Uply.Infrastructure.Database;

namespace Uply.Web.Extensions;

public static class WebApplicationExtensions
{
    public static void ApplyDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var migrator = scope.ServiceProvider.GetService<IDatabaseMigrator>();
        migrator!.Migrate();
    }

    public static void AddCors(this WebApplication app)
    {
        var allowedOrigins = app.Configuration["ALLOWED_ORIGINS"]?.Split(",") ?? [];

        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetService<ILogger<WebApplication>>();

        Log.Logger.Information("CORS Added for origins: {origins}", string.Join(", ", allowedOrigins));

        app.UseCors(options =>
            options
                .WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
        );
    }
}
