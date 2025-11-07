using Domain.Entities.Abstract;
using MongoDB.Driver;
using Uply.Domain.Abstractions.Repositories.Abstract;

namespace Uply.Infrastructure.MongoDatabase.Repositories.Abstract;

public abstract class CrudRepository<TEntity>(MongoDbContext dbContext) : ICrudRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly IMongoCollection<TEntity> _collection = dbContext.GetCollection<TEntity>();

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        => await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _collection.Find(_ => true).ToListAsync();

    public virtual async Task CreateAsync(TEntity entity)
        => await _collection.InsertOneAsync(entity);

    public virtual async Task UpdateAsync(TEntity entity)
        => await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);

    public virtual async Task DeleteAsync(Guid id)
        => await _collection.DeleteOneAsync(e => e.Id == id);
}
