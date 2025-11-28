using Uply.Domain.Models.Dto.UserDtos;

namespace Uply.Domain.Abstractions.Services;

public interface IUserService
{
    Task<UserDto?> GetUserWithRoadmaps(Guid userId);
	Task<UserProgressDto?> GetUserWithRoadmapProgress(Guid userId);
}
