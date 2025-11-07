using Domain.Entities;
using Microsoft.Extensions.Options;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Abstractions.Services.Telegram;
using Uply.Domain.Settings;

namespace Uply.Domain.Services;

public class AuthService(
    IUserRepository userRepository,
    IOptions<TelegramSettings> options,
    ITelegramAuthVerifyService telegramAuthVerifyService) : IAuthService
{
    private readonly TelegramSettings _settings = options.Value;

    public async Task<User> AuthorizeAsync(string initDataRaw)
    {
        if (!telegramAuthVerifyService.Verify(initDataRaw, _settings.BotToken))
        {
            throw new UnauthorizedAccessException("Invalid Telegram signature");
        }

        var tgUser = telegramAuthVerifyService.ParseUser(initDataRaw);

        var user = await userRepository.GetByTelegramIdAsync(tgUser.Id);
        if (user is not null)
        {
            return user;
        }

        user = new User
        {
            TelegramId = tgUser.Id,
            Username = tgUser.Username,
            FirstName = tgUser.FirstName,
            LastName = tgUser.LastName,
            CreatedAt = DateTime.UtcNow,
            LastLoginAt = DateTime.UtcNow
        };

        await userRepository.CreateAsync(user);
        return user;
    }
}
