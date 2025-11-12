namespace Uply.Domain.Models.Dto.RoadmapTasks.Commands;

public record CreateRoadmapTaskDto
{
    public Guid RoadmapId { get; init; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public int TaskNumber { get; init; }
}
