using Application.Abstractions.Services.Minio;
using Infrastructure.Minio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Services._Minio_;
using Uply.Infrastructure.ChatGpt;
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

        services.AddChatGpt(config);

        services.AddFileStorage();

        return services;
    }

    private static IServiceCollection AddFileStorage(this IServiceCollection services)
    {
        services.AddMinio(configureClient =>
        {
            var useSSL = bool.Parse(Environment.GetEnvironmentVariable("MINIO_USE_SSL")!);
            var host = Environment.GetEnvironmentVariable("MINIO_HOST")!;
            var port = int.Parse(Environment.GetEnvironmentVariable("MINIO_PORT")!);
            var accessKey = Environment.GetEnvironmentVariable("MINIO_SERVER_ACCESS_KEY")!;
            var secretKey = Environment.GetEnvironmentVariable("MINIO_SERVER_SECRET_KEY")!;

            configureClient
                .WithSSL(useSSL)
                .WithEndpoint(host, port)
                .WithCredentials(accessKey, secretKey)
                .Build();
        });

        services.AddScoped<IMinioService, MinioService>();
        services.AddScoped<IMinioBucketBuilder, MinioBucketBuilder>();

        return services;
    }

    private static IServiceCollection AddChatGpt(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient();
        services.Configure<ChatGptOptions>(opt =>
        {
            opt.Model = Environment.GetEnvironmentVariable("CHATGPT_MODEL")!;
            opt.ApiKey = Environment.GetEnvironmentVariable("CHATGPT_APIKEY")!;
        });

        services.AddSingleton<IChatGptClient, ChatGptClient>();

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
        services.AddScoped<ITaskReportRepository, TaskReportRepository>();
        services.AddScoped<ISkinRepository, SkinRepository>();

        return services;
    }
}
