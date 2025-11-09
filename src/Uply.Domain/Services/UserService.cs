using MapsterMapper;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Models.Dto.UserDtos;

namespace Uply.Domain.Services;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    public async Task<UserDto?> GetUserWithRoadmaps(Guid userId)
    {
        var user = await userRepository.GetWithRoadmapsAsync(userId);
        if (user == null)
        {
            return null;
        }

        return mapper.Map<UserDto>(user);
    }
}
