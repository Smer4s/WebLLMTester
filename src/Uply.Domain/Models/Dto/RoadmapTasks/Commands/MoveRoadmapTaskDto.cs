namespace Uply.Domain.Models.Dto.RoadmapTasks.Commands;

public record MoveRoadmapTaskDto
{
    public Guid RoadmapId { get; set; }
    public Guid RoadmapTaskId { get; set; }
    public int NewPosition { get; set; }
}
