using Domain.Entities;
using MongoDB.Driver;
using Uply.Domain.Abstractions.Repositories;
using Uply.Infrastructure.MongoDatabase.Repositories.Abstract;

namespace Uply.Infrastructure.MongoDatabase.Repositories;

public class RoadmapTaskRepository(MongoDbContext dbContext) : CrudRepository<RoadmapTask>(dbContext), IRoadmapTaskRepository;

