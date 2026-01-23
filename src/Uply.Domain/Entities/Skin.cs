using Uply.Domain.Entities.Abstract;
using Uply.Domain.Enums;

namespace Uply.Domain.Entities;

public class Skin : BaseEntity
{
    public string Description { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
    public int Price { get; set; }
    public SkinType SkinType { get; set; }
}