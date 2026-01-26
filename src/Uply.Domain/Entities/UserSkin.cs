using Uply.Domain.Entities.Abstract;

namespace Uply.Domain.Entities;

public class UserSkin : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid SkinId { get; set; }
    public Skin Skin { get; set; } = null!;
    
    public bool IsUsed { get; set; }
}