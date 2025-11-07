using Domain.Entities;

namespace Uply.Domain.Abstractions.Services;

public interface IAuthService
{
    Task<User> AuthorizeAsync(string initDataRaw);
}

