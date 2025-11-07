using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Uply.Infrastructure.MongoDatabase;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<TEntity> GetCollection<TEntity>() => _database.GetCollection<TEntity>(typeof(TEntity).Name);

    public IMongoCollection<User> Users => _database.GetCollection<User>(nameof(User));
    public IMongoCollection<Roadmap> Roadmaps => _database.GetCollection<Roadmap>(nameof(Roadmap));
    public IMongoCollection<RoadmapTask> RoadmapTasks => _database.GetCollection<RoadmapTask>(nameof(RoadmapTask));
}
