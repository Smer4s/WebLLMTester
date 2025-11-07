using Domain.Entities.Abstract;

namespace Domain.Entities;

public class RoadmapTask : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsCompleted { get; set; }
}
