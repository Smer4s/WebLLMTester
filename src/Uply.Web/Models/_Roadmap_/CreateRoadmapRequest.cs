using Uply.Domain.Enums;

namespace Uply.Web.Models._Roadmap_;

public class CreateRoadmapRequest
{
    public string Title { get; set; } = null!;
    public string StartingPoint { get; set; } = null!;
    public string Goal { get; set; } = null!;
    public DateOnly? Deadline { get; set; }
    public Period Period { get; set; }
    public ManHoursPerTask ManHoursPerTask { get; set; }
}
