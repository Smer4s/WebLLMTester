using Mapster;
using Uply.Domain.Entities;
using Uply.Domain.Enums;
using Uply.Domain.Models.Dto.RoadmapTasks;

namespace Uply.Domain.Models.Dto.Roadmaps;

public class RoadmapFullDto : RoadmapBaseDto
{
    public Guid IssuerId { get; set; }

    public List<RoadmapTaskSlimDto> Tasks { get; set; } = [];

    public void ConfigureMapping(TypeAdapterConfig config)
    {
        config.NewConfig<Roadmap, RoadmapFullDto>()
            .Map(dest => dest.Tasks, src => src.RoadmapTasks.OrderedTasks)
            .RequireDestinationMemberSource(true);
    }
}
