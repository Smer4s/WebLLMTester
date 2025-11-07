using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uply.Domain.Abstractions.Repositories;
using Uply.Infrastructure.MongoDatabase;
using Uply.Infrastructure.MongoDatabase.Repositories;

namespace Uply.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddMongoDatabase(config);

        return services;
    }

    private static IServiceCollection AddMongoDatabase(this IServiceCollection services, IConfiguration config)
    {
        var section = config.GetRequiredSection("MongoDbSettings");

        services.Configure<MongoDbSettings>(section);
        services.AddScoped<MongoDbContext>();

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
