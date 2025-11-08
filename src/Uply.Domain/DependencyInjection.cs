using Mapster;
using Mapster.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
        services.AddUplyAuthorization(configuration);
        services.Configure<TelegramSettings>(configuration.GetRequiredSection("Telegram"));
        services.AddDomainServices();

        services.AddMapster();
        TypeAdapterConfig.GlobalSettings.ScanInheritedTypes(typeof(DependencyInjection).Assembly);

        return services;
    }

    private static IServiceCollection AddUplyAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetRequiredSection("Jwt");
        services.Configure<JwtSettings>(jwtSection);

        var jwtSettings = jwtSection.Get<JwtSettings>()!;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
        });

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITelegramAuthVerifyService, TelegramAuthVerifyService>();

        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoadmapService, RoadmapService>();

        return services;
    }
}
