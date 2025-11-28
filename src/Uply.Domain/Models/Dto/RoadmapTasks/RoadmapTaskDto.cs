using Mapster;
using Uply.Domain.Entities;

namespace Uply.Domain.Models.Dto.RoadmapTasks;

public class RoadmapTaskDto : IMapFrom<RoadmapTask>
{
    public Guid Id { get; set; }
    public Guid RoadmapId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsCompleted { get; set; }

	public int TaskNumber { get; set; }

	public TaskReportDto? TaskReport { get; set; }

    public void ConfigureMapping(TypeAdapterConfig config)
    {
        config.NewConfig<RoadmapTask, RoadmapTaskDto>()
            .Map(dest => dest.IsCompleted, src => src.IsCompleted)
            .Map(dest => dest.TaskReport, src => src.TaskReport);
    }
}
