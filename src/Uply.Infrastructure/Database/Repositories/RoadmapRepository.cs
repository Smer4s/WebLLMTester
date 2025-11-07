using MongoDB.Driver;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Entities;
using Uply.Infrastructure.Database.Repositories.Abstract;

namespace Uply.Infrastructure.Database.Repositories;

public class RoadmapRepository(AppDbContext context) : CrudRepository<Roadmap>(context), IRoadmapRepository;

