namespace Uply.Domain.Models.Telegram;

public record TelegramUserInfo
{
    public long Id { get; set; }
    public string? Username { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
}
