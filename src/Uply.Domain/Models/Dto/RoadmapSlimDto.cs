using Mapster;
using Uply.Domain.Entities;
using Uply.Domain.Enums;

namespace Uply.Domain.Models.Dto;

public class RoadmapSlimDto : IMapFrom<Roadmap>
{
    public Guid Id { get; set; }
    public required string StartingPoint { get; set; }
    public required string Goal { get; set; }
    public DateOnly? Deadline { get; set; }
    public Period Period { get; set; }
    public ManHoursPerTask ManHoursPerTask { get; set; }

    public void ConfigureMapping(TypeAdapterConfig config)
    {
        config.NewConfig<Roadmap, RoadmapSlimDto>();
    }
}
