using Mapster;
using Uply.Domain.Entities;

namespace Uply.Domain.Models.Dto;

public class RoadmapTaskDto : IMapFrom<RoadmapTask>
{
    public Guid Id { get; set; }
    public Guid RoadmapId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsCompleted { get; set; }
}
