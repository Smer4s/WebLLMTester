using Uply.Domain.Models.Telegram;
namespace Uply.Domain.Abstractions.Services.Telegram;

public interface ITelegramAuthVerifyService
{
    TelegramUserInfo ParseUser(string initDataRaw);
    bool Verify(string initDataRaw, string botToken);
}
