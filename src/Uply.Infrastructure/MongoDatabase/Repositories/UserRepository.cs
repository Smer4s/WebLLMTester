using Domain.Entities;
using MongoDB.Driver;
using Uply.Domain.Abstractions.Repositories;
using Uply.Infrastructure.MongoDatabase.Repositories.Abstract;

namespace Uply.Infrastructure.MongoDatabase.Repositories;

public class UserRepository(MongoDbContext database) : CrudRepository<User>(database), IUserRepository;