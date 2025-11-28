using Microsoft.EntityFrameworkCore;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Entities;
using Uply.Infrastructure.Database.Repositories.Abstract;

namespace Uply.Infrastructure.Database.Repositories;

public class RoadmapRepository(AppDbContext context) : CrudRepository<Roadmap>(context), IRoadmapRepository
{
	public async Task<Roadmap?> GetRoadmapByIdAsyncWithIncludes(Guid id)
	{
		return await _dbSet
			.Include(x => EF.Property<List<RoadmapTask>>(x, "_tasks"))
			.ThenInclude(x => x.TaskReport)
			.FirstOrDefaultAsync(x => x.Id == id);
	}
}