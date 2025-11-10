using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Uply.Domain.Abstractions.Services.Telegram;
using Uply.Domain.Models.Telegram;

namespace Uply.Domain.Services.Telegram;

public class TelegramAuthVerifyService(ILogger<TelegramAuthVerifyService> logger) : ITelegramAuthVerifyService
{
    public bool Verify(string initDataRaw, string botToken)
    {
        var decoded = Uri.UnescapeDataString(initDataRaw);
        logger.LogInformation("Decoded initDataRaw: {decoded}", decoded);

        var secretKey = SHA256.HashData(Encoding.UTF8.GetBytes(botToken));
        using var hmac = new HMACSHA256(secretKey);

        var parts = decoded
             .Split('&', StringSplitOptions.RemoveEmptyEntries)
             .Select(p => p.Split('=', 2))
             .Where(p => p.Length == 2)
             .ToDictionary(
                 p => p[0],
                 p => Uri.UnescapeDataString(p[1]).Replace(@"\/", "/") 
             );

        logger.LogInformation("Parsed parts: {parts}", string.Join(", ", parts.Select(kv => $"{kv.Key}={kv.Value}")));

        var dataCheckString = string.Join("\n",
            parts.Where(kv => kv.Key != "hash" && kv.Key != "signature")
                 .OrderBy(kv => kv.Key)
                 .Select(kv => $"{kv.Key}={kv.Value}"));


        logger.LogInformation("DataCheckString: {dataCheckString}", dataCheckString);

        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataCheckString));
        var computedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        logger.LogInformation("ComputedHash: {computedHash}", computedHash);

        if (!parts.TryGetValue("hash", out var receivedHash))
        {
            logger.LogWarning("No hash found in parts");
            return false;
        }

        logger.LogInformation("ReceivedHash: {receivedHash}", receivedHash);

        var result = computedHash == receivedHash;
        logger.LogInformation("Verification result: {result}", result);

        return result;
    }


    public TelegramUserInfo ParseUser(string initDataRaw)
    {
        var decoded = Uri.UnescapeDataString(initDataRaw);

        var dict = decoded
            .Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Split('=', 2))
            .Where(p => p.Length == 2)
            .ToDictionary(p => p[0], p => Uri.UnescapeDataString(p[1]));

        if (!dict.TryGetValue("user", out var userJson))
            throw new InvalidOperationException("User data not found in initDataRaw");

        var user = JsonSerializer.Deserialize<TelegramUserInfo>(userJson);

        if (user == null)
            throw new InvalidOperationException("Failed to parse user JSON");

        return user;
    }


}
