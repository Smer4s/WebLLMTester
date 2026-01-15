using Mapster;
using Uply.Domain.Entities;

namespace Uply.Domain.Models.Dto.RoadmapTasks;

public record RoadmapTaskSlimDto : IMapFrom<RoadmapTask>
{
    public Guid Id { get; set; }
    public Guid RoadmapId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int TaskNumber { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsActiveTask { get; set; }
}