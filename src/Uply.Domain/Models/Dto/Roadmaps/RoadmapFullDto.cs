using Mapster;
using Uply.Domain.Entities;
using Uply.Domain.Enums;
using Uply.Domain.Models.Dto.RoadmapTasks;

namespace Uply.Domain.Models.Dto.Roadmaps;

public class RoadmapFullDto : IMapFrom<Roadmap>
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

    public List<RoadmapTaskSlimDto> Tasks { get; set; } = [];

    public void ConfigureMapping(TypeAdapterConfig config)
    {
        config.NewConfig<Roadmap, RoadmapFullDto>()
            .Map(dest => dest.Tasks, src => src.RoadmapTasks.OrderedTasks)
			.Map(dest => dest.CurrentTask, src => src.RoadmapTasks.CurrentTask)
			.Map(dest => dest.NextTask, src => src.RoadmapTasks.NextTask)
			.RequireDestinationMemberSource(true);
    }
}
