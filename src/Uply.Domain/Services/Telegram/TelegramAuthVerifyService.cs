using System.Security.Cryptography;
using System.Text;
using Uply.Domain.Abstractions.Services.Telegram;
using Uply.Domain.Models.Telegram;

namespace Uply.Domain.Services.Telegram;

public class TelegramAuthVerifyService : ITelegramAuthVerifyService
{
    public  bool Verify(string initDataRaw, string botToken)
    {
        var secretKey = SHA256.HashData(Encoding.UTF8.GetBytes(botToken));
        using var hmac = new HMACSHA256(secretKey);

        var parts = initDataRaw.Split('&');
        var dataCheckString = string.Join("\n",
            parts.Where(p => !p.StartsWith("hash="))
                 .OrderBy(p => p));

        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataCheckString));
        var computedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        var receivedHash = parts.First(p => p.StartsWith("hash=")).Split('=')[1];

        return computedHash == receivedHash;
    }

    public  TelegramUserInfo ParseUser(string initDataRaw)
    {
        var dict = initDataRaw.Split('&')
            .Select(p => p.Split('='))
            .ToDictionary(p => p[0], p => Uri.UnescapeDataString(p[1]));

        return new TelegramUserInfo
        {
            Id = long.Parse(dict["user.id"]),
            Username = dict.TryGetValue("user.username", out string? username) ? username : null,
            FirstName = dict["user.first_name"],
            LastName = dict.TryGetValue("user.last_name", out string? lastName) ? lastName : null
        };
    }
}
