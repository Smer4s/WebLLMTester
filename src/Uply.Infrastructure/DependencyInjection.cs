using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Abstractions.Services;
using Uply.Infrastructure.Database;
using Uply.Infrastructure.Database.Migrator;
using Uply.Infrastructure.Database.Repositories;

namespace Uply.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDatabase(config);

        services.AddScoped<IDatabaseMigrator, DatabaseMigrator>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") is "Development")
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("Postgres")));
        }

        else
        {
            var host = Environment.GetEnvironmentVariable("PGHOST");
            var port = Environment.GetEnvironmentVariable("PGPORT");
            var user = Environment.GetEnvironmentVariable("PGUSER");
            var password = Environment.GetEnvironmentVariable("PGPASSWORD");
            var database = Environment.GetEnvironmentVariable("PGDATABASE");

            var connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={database};SslMode=Require";

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        services.AddRepositories();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoadmapRepository, RoadmapRepository>();
        services.AddScoped<IRoadmapTaskRepository, RoadmapTaskRepository>();

        return services;
    }
}
