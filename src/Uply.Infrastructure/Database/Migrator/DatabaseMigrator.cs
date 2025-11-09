using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Uply.Domain.Abstractions.Services;

namespace Uply.Infrastructure.Database.Migrator;

public class DatabaseMigrator(AppDbContext dbContext, ILogger<DatabaseMigrator> logger)
    : IDatabaseMigrator
{
    public void Migrate()
    {
        try
        {
            dbContext.Database.Migrate();
            logger.LogInformation("Database has been successfully migrated...");
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Exception occured while database migration:\r\n{message}", e.Message);
            throw;
        }
    }
}
