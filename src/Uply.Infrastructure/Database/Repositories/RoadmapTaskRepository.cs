using Microsoft.EntityFrameworkCore;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Entities;
using Uply.Infrastructure.Database.Repositories.Abstract;

namespace Uply.Infrastructure.Database.Repositories;

public class RoadmapTaskRepository(AppDbContext dbContext) : CrudRepository<RoadmapTask>(dbContext), IRoadmapTaskRepository
{
    public async Task<RoadmapTask?> GetByIdWithIncludes(Guid id)
    {
        var query = _dbSet
            .Include(x => x.Roadmap)
            .ThenInclude(x => EF.Property<List<RoadmapTask>>(x, "_tasks"))
            .Include(x => x.TaskReport);

        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }
}

