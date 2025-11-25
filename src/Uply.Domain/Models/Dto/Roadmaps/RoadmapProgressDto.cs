using Mapster;
using Uply.Domain.Entities;
using Uply.Domain.Enums;
using Uply.Domain.Models.Dto.RoadmapTasks;

namespace Uply.Domain.Models.Dto.Roadmaps;

public record RoadmapProgressDto : IMapFrom<Roadmap>
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public Guid IssuerId { get; set; }
    public string StartingPoint { get; set; } = null!;
    public string Goal { get; set; } = null!;
    public DateOnly? Deadline { get; set; }
    public Period Period { get; set; }
    public ManHoursPerTask ManHoursPerTask { get; set; }

    public int Days { get; init; }

    public List<RoadmapTaskDto> Tasks { get; set; } = [];
    public int TotalTaskCount { get; init; }
    public int CompletedCount { get; init; }

    public void ConfigureMapping(TypeAdapterConfig config)
    {
        config.NewConfig<Roadmap, RoadmapProgressDto>()
            .Map(dest => dest.Tasks, src => src.RoadmapTasks.OrderedTasks)
            .Map(dest => dest.Days, src => (int)(DateTime.UtcNow - src.Start).TotalDays)
            .Map(dest => dest.TotalTaskCount, src => src.RoadmapTasks.TotalTaskCount)
            .Map(dest => dest.CompletedCount, src => src.RoadmapTasks.CompletedCount)
            .RequireDestinationMemberSource(true);
    }
}