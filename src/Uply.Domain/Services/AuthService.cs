using MapsterMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Abstractions.Services.Telegram;
using Uply.Domain.Entities;
using Uply.Domain.Models.Dto;
using Uply.Domain.Models.Dto.UserDtos;
using Uply.Domain.Models.Telegram;
using Uply.Domain.Settings;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Uply.Domain.Services;

public class AuthService(
    IUserRepository userRepository,
    IOptions<TelegramSettings> options,
    IOptions<JwtSettings> jwtOptions,
    ILogger<AuthService> logger,
    IMapper mapper,
    ITelegramAuthVerifyService telegramAuthVerifyService) : IAuthService
{
    private readonly TelegramSettings _settings = options.Value;

    public async Task<AuthResultDto> AuthorizeAsync(string initDataRaw)
    {
        if (!telegramAuthVerifyService.Verify(initDataRaw, _settings.BotToken))
        {
            //throw new UnauthorizedAccessException("Invalid Telegram signature");
            logger.LogCritical("Invalid Telegram signature!!!");
        }

        var tgUser = telegramAuthVerifyService.ParseUser(initDataRaw);

        //var tgUser = new TelegramUserInfo()
        //{
        //    FirstName = "first_name",
        //    Id = 228,
        //    LastName = "lastnam",
        //    Username = "@example"
        //};

        var user = await userRepository.GetByTelegramIdAsync(tgUser.Id);
        if (user is null)
        {
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
        }

        return GenerateTokens(user);
    }

    public async Task<AuthResultDto> RefreshAsync(string refreshToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));

        try
        {
            var principal = handler.ValidateToken(refreshToken, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Value.Issuer,
                ValidAudience = jwtOptions.Value.Audience,
                IssuerSigningKey = key,
                ValidateLifetime = true
            }, out var validatedToken);

            var userId = (principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value) ?? throw new UnauthorizedAccessException("Invalid refresh token");

            var user = await userRepository.GetByIdAsync(Guid.Parse(userId)) ?? throw new UnauthorizedAccessException("User not found");

            return GenerateTokens(user);
        }
        catch
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }
    }

    private AuthResultDto GenerateTokens(User user)
    {
        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("telegram_id", user.TelegramId.ToString()),
                new Claim(ClaimTypes.Name, user.Username ?? user.FirstName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtOptions.Value.AccessTokenExpiresMinutes),
            signingCredentials: creds);

        var refreshToken = new JwtSecurityToken(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpiresDays),
            signingCredentials: creds);

        var userDto = mapper.Map<SlimUserDto>(user);

        return new AuthResultDto
        {
            User = userDto,
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken)
        };
    }
}

