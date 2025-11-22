namespace Uply.Web.Models._TaskReport_;

public class CreateRoadmapReportDto
{
    public Guid RoadmapTaskId { get; set; }
    public string? Description { get; set; }
    public ICollection<string> PhotoUrls { get; set; } = [];
}
