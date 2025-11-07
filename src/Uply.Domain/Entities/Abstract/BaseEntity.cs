using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.Abstract;

public abstract class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)] 
    public Guid Id { get; set; }
}
