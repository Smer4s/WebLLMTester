using Uply.Domain.Enums;

namespace Uply.Domain.Models._Skins_;

public record CreateSkinDto
{
    public string Description { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string Url { get; init; } = null!;
    public uint Price { get; init; }
    public SkinType SkinType { get; init; }
}