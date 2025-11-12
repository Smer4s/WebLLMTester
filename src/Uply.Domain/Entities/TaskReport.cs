using Uply.Domain.Entities.Abstract;

namespace Uply.Domain.Entities;

public class TaskReport : BaseEntity
{
    public string? Description { get; set; }
    public ICollection<string> PhotoUrls { get; set; } = [];

    public RoadmapTask Task { get; set; } = null!;
}
