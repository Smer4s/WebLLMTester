using Mapster;
using Uply.Domain.Enums;

namespace Uply.Domain.Models.Dto.Skin;

public record SkinDto : IMapFrom<Entities.Skin>
{
    public string Description { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string Url { get; init; } = null!;
    public uint Price { get; init; }
    public SkinType SkinType { get; init; }
    
    public void ConfigureMapping(TypeAdapterConfig config)
    {
        config.NewConfig<Entities.Skin, SkinDto>()
            .RequireDestinationMemberSource(true);
    }
}