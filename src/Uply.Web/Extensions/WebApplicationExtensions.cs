using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
}
