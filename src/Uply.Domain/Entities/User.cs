using Uply.Domain.Entities.Abstract;

namespace Uply.Domain.Entities;

public class User : BaseEntity
{
    public long TelegramId { get; set; }

    /// <summary>
    ///     Логин пользователя (в телеграме @username)
    /// </summary>
    public string? Username { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginAt { get; set; }
    public string? PhotoUrl { get; set; }

    public ICollection<Roadmap> Roadmaps { get; set; } = [];
}
