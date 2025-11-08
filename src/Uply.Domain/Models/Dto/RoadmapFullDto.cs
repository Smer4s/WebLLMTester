using Mapster;
using Uply.Domain.Entities;
using Uply.Domain.Enums;

namespace Uply.Domain.Models.Dto;

public class RoadmapFullDto : IMapFrom<Roadmap>
{
    public Guid Id { get; set; }
    public Guid IssuerId { get; set; }
    public string StartingPoint { get; set; } = null!;
    public string Goal { get; set; } = null!;
    public DateOnly? Deadline { get; set; }
    public Period Period { get; set; }
    public ManHoursPerTask ManHoursPerTask { get; set; }
    public ICollection<RoadmapTaskDto> Tasks { get; set; } = [];

    public void ConfigureMapping(TypeAdapterConfig config)
    {
        config.NewConfig<Roadmap, RoadmapFullDto>()
            .Map(dest => dest.Tasks, src => src.Tasks)
            .RequireDestinationMemberSource(true);
    }
}
