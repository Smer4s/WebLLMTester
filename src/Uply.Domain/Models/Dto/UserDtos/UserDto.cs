using Mapster;
using Uply.Domain.Entities;
using Uply.Domain.Models.Dto.Roadmaps;

namespace Uply.Domain.Models.Dto.UserDtos;

public class UserDto : IMapFrom<User>
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhotoUrl { get; set; }
    public uint GoldAmount { get; set; }

    public ICollection<RoadmapSlimDto> Roadmaps { get; set; } = [];

    public void ConfigureMapping(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>()
            .Map(dest => dest.Roadmaps, src => src.Roadmaps);
    }
}
