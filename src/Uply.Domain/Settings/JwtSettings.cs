namespace Uply.Domain.Settings;

public class JwtSettings
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string Key { get; set; } = null!;
    public int AccessTokenExpiresMinutes { get; set; }
    public int RefreshTokenExpiresDays { get; set; }
}
