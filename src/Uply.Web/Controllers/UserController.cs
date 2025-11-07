using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uply.Domain.Abstractions.Services;
using Uply.Web.Controllers.Abstract;
using Uply.Web.Extensions;

namespace Uply.Web.Controllers;

public class UserController(IUserService userService) : RestApiController
{
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetUserWithRoadmaps()
    {
        var id = User.GetId();

        var user = await userService.GetUserWithRoadmaps(id);

        return Ok(user);
    }
}
