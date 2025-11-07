using Domain.Entities;
using Uply.Domain.Abstractions.Repositories;
using Uply.Infrastructure.Database;
using Uply.Infrastructure.Database.Repositories.Abstract;

namespace Uply.Infrastructure.Database.Repositories;

public class UserRepository(AppDbContext dbContext) : CrudRepository<User>(dbContext), IUserRepository;