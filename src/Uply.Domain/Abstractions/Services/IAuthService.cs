using Uply.Domain.Models.Dto;

namespace Uply.Domain.Abstractions.Services;

public interface IAuthService
{
    Task<AuthResultDto> AuthorizeAsync(string initDataRaw);
    Task<AuthResultDto> RefreshAsync(string refreshToken);
}
