using Domain.Entities;
using Uply.Domain.Abstractions.Repositories.Abstract;

namespace Uply.Domain.Abstractions.Repositories;

public interface IUserRepository : ICrudRepository<User>;
