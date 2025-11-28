using Mapster;
using Uply.Domain.Entities;
using Uply.Domain.Enums;
using Uply.Domain.Models.Dto.RoadmapTasks;

namespace Uply.Domain.Models.Dto.Roadmaps;

public class RoadmapSlimDto : RoadmapBaseDto, IMapFrom<Roadmap>
{
	public void ConfigureMapping(TypeAdapterConfig config)
    {
        config.NewConfig<Roadmap, RoadmapSlimDto>()
            .Map(dest => dest.CurrentTask, src => src.RoadmapTasks.CurrentTask)
            .Map(dest => dest.NextTask, src => src.RoadmapTasks.NextTask);
    }
}
