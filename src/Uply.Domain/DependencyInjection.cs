using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Abstractions.Services.Telegram;
using Uply.Domain.Services;
using Uply.Domain.Services.Telegram;
using Uply.Domain.Settings;

namespace Uply.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramSettings>(configuration.GetSection("Telegram"));
        services.AddDomainServices();

        return services;
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITelegramAuthVerifyService, TelegramAuthVerifyService>();

        return services;
    }
}
