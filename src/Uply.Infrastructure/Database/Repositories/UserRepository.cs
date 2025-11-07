using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Uply.Domain.Abstractions.Repositories;
using Uply.Infrastructure.Database;
using Uply.Infrastructure.Database.Repositories.Abstract;

namespace Uply.Infrastructure.Database.Repositories;

public class UserRepository(AppDbContext dbContext) : CrudRepository<User>(dbContext), IUserRepository
{
    public async Task<User?> GetByTelegramIdAsync(long telegramId) =>
        await _dbSet.FirstOrDefaultAsync(u => u.TelegramId == telegramId);
}