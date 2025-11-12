using Mapster;
using Uply.Domain.Entities;

namespace Uply.Domain.Models.Dto.RoadmapTasks;

public class TaskReportDto : IMapFrom<TaskReport>
{
    public string? Description { get; set; }
    public ICollection<string> PhotoUrls { get; set; } = [];
}
