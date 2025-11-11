namespace Uply.Domain.Models.Dto.RoadmapTasks.Commands;

public class UpdateRoadmapTaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}
