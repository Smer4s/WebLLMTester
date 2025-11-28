using Mapster;
using Uply.Domain.Entities;
using Uply.Domain.Models.Dto.RoadmapTasks;

namespace Uply.Domain.Models.Dto.Roadmaps;

public class RoadmapProgressDto : RoadmapBaseDto
{
	public Guid IssuerId { get; set; }

	public int Days { get; init; }

	public List<RoadmapTaskDto> Tasks { get; set; } = [];
	public int TotalTaskCount { get; init; }
	public int CompletedCount { get; init; }
	public int CompletedPercent { get; init; }

	public void ConfigureMapping(TypeAdapterConfig config)
	{
		config.NewConfig<Roadmap, RoadmapProgressDto>()
			.Map(dest => dest.Tasks, src => src.RoadmapTasks.OrderedTasks)
			.Map(dest => dest.Days, src => (int)(DateTime.UtcNow - src.Start).TotalDays)
			.Map(dest => dest.TotalTaskCount, src => src.RoadmapTasks.TotalTaskCount)
			.Map(dest => dest.CompletedCount, src => src.RoadmapTasks.CompletedCount)
			.Map(dest => dest.CompletedPercent, src => src.RoadmapTasks.CompletedPercent)
			.RequireDestinationMemberSource(true);
	}
}