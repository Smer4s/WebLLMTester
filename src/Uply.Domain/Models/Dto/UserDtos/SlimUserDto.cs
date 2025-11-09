using Mapster;
using Uply.Domain.Entities;

namespace Uply.Domain.Models.Dto.UserDtos;

public record SlimUserDto : IMapFrom<User>
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
}
