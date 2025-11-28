using Uply.Domain.Abstractions.Repositories.Abstract;
using Uply.Domain.Entities;

namespace Uply.Domain.Abstractions.Repositories;

public interface IUserRepository : ICrudRepository<User>
{
    Task<User?> GetByTelegramIdAsync(long telegramId);
    Task<User?> GetWithRoadmapsAsync(Guid userId);
	Task<User?> GetWithRoadmapsProgressAsync(Guid userId);
}
