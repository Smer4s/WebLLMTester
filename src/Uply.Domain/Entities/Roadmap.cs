using Uply.Domain.Common;
using Uply.Domain.Entities.Abstract;
using Uply.Domain.Enums;

namespace Uply.Domain.Entities;

public class Roadmap(List<RoadmapTask> tasks) : BaseEntity
{
    protected Roadmap() : this([]) { }

    private readonly List<RoadmapTask> _tasks = tasks;

    public Guid IssuerId { get; set; }
    public User Issuer { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string StartingPoint { get; set; } = null!;
    public string Goal { get; set; } = null!;
    public DateOnly? Deadline { get; set; }
    public Period Period { get; set; }
    public ManHoursPerTask ManHoursPerTask { get; set; }
    public DateTime Start { get; set; }

    public RoadmapTaskCollection RoadmapTasks => new(_tasks);
}