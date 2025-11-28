using Mapster;
using Uply.Domain.Entities;
using Uply.Domain.Enums;
using Uply.Domain.Models.Dto.RoadmapTasks;

namespace Uply.Domain.Models.Dto.Roadmaps;

public class RoadmapProgressDto : IMapFrom<Roadmap>
{
	public Guid Id { get; set; }
	public required string StartingPoint { get; set; }
	public required string Title { get; set; }
	public required string Goal { get; set; }
	public DateOnly? Deadline { get; set; }
	public RoadmapTaskDto? CurrentTask { get; set; }
	public ManHoursPerTask ManHoursPerTask { get; set; }
	public RoadmapTaskDto? NextTask { get; set; }
	public Period Period { get; set; }

	public bool IsDuo { get; set; }

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
			.Map(dest => dest.CurrentTask, src => src.RoadmapTasks.CurrentTask)
			.Map(dest => dest.NextTask, src => src.RoadmapTasks.NextTask)
			.RequireDestinationMemberSource(true);
	}
}