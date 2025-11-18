using System.Text.Json.Serialization;

namespace Uply.Domain.Models.Telegram;

public record TelegramUserInfo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = null!;

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("photo_url")]
    public string? PhotoUrl { get; set; }
}