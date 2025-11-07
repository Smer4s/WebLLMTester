using Uply.Domain.Models.Dto.UserDtos;

namespace Uply.Domain.Models.Dto;

public class AuthResultDto
{
    public SlimUserDto User { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
