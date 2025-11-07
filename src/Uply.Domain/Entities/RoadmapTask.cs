using Uply.Domain.Entities.Abstract;

namespace Uply.Domain.Entities;

public class RoadmapTask : BaseEntity
{
    public Guid RoadmapId { get; set; }
    public Roadmap Roadmap { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsCompleted { get; set; }
}
