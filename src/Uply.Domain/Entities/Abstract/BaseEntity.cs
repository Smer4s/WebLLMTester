namespace Uply.Domain.Entities.Abstract;

public abstract class BaseEntity
{
    public Guid Id { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is BaseEntity entity && this.Id == entity.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
