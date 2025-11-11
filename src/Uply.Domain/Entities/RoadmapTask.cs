using Uply.Domain.Abstractions;
using Uply.Domain.Entities.Abstract;

namespace Uply.Domain.Entities;

public class RoadmapTask : BaseEntity, ITaskState
{
    public Guid RoadmapId { get; set; }
    public Roadmap Roadmap { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsCompleted => TaskReportId.HasValue;
    public int TaskNumber { get; set; }

    public TaskReport? TaskReport { get; set; }
    public Guid? TaskReportId { get; set; }
}
