using Uply.Domain.Entities.Abstract;
using Uply.Domain.Enums;

namespace Uply.Domain.Entities;

public class Roadmap : BaseEntity
{
    public Guid IssuerId { get; set; }
    public User Issuer { get; set; } = null!;
    public string StartingPoint { get; set; } = null!;
    public string Goal { get; set; } = null!;
    public DateOnly? Deadline { get; set; }
    public Period Period { get; set; }
    public ManHoursPerTask ManHoursPerTask { get; set; }
    public ICollection<RoadmapTask> Tasks { get; set; } = [];
}

