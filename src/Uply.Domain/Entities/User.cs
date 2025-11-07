using Domain.Entities.Abstract;

namespace Domain.Entities;

public class User : BaseEntity
{
    public long TelegramId { get; set; }

    /// <summary>
    ///     Логин пользователя (в телеграме @username)
    /// </summary>
    public string? Username { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;

    public ICollection<Roadmap> Roadmaps { get; set; } = [];
}
