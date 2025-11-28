using Microsoft.EntityFrameworkCore;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Entities;
using Uply.Infrastructure.Database.Repositories.Abstract;

namespace Uply.Infrastructure.Database.Repositories;

public class UserRepository(AppDbContext dbContext) : CrudRepository<User>(dbContext), IUserRepository
{
	public async Task<User?> GetByTelegramIdAsync(long telegramId) =>
		await _dbSet.FirstOrDefaultAsync(u => u.TelegramId == telegramId);

	public Task<User?> GetWithRoadmapsAsync(Guid userId)
		=> _dbSet.Include(x => x.Roadmaps)
			.ThenInclude(x => EF.Property<List<RoadmapTask>>(x, "_tasks"))
			.FirstOrDefaultAsync(x => x.Id == userId);

	public Task<User?> GetWithRoadmapsProgressAsync(Guid userId)
	{
		var query = _dbSet.AsQueryable();

		query = query.Include(x => x.Roadmaps.OrderByDescending(x => x.LastRoadmapActivity))
				.ThenInclude(x => EF.Property<List<RoadmapTask>>(x, "_tasks"))
				.ThenInclude(x => x.TaskReport);
		
		return query
				.FirstOrDefaultAsync(x => x.Id == userId);
	}
}