using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Uply.Domain.Abstractions.Repositories;
using Uply.Web.Controllers.Abstract;

namespace Uply.Web.Controllers;

public class AuthController(IUserRepository userRepository) : RestApiController
{
    [HttpGet]
    public async Task<IActionResult> Authorize()
    {
        var user = new User()
        {
            Username = "admin",
            CreatedAt = DateTime.Now,
            LastLoginAt = DateTime.Now,
            FirstName = "admin",
            LastName = "admin",
        };

        await userRepository.CreateAsync(user);

        var userFromDb = await userRepository.GetByIdAsync(user.Id);

        return Ok(userFromDb);
    }
}
