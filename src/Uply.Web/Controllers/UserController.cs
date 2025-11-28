using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Models.Dto.Roadmaps;
using Uply.Domain.Models.Dto.UserDtos;
using Uply.Web.Controllers.Abstract;
using Uply.Web.Extensions;

namespace Uply.Web.Controllers;

public class UserController(IUserService userService) : RestApiController
{
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserWithRoadmaps()
    {
        var id = User.GetId();

        var user = await userService.GetUserWithRoadmaps(id);

        return Ok(user);
    }

	[Authorize]
	[HttpGet("me/progress")]
	[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetUserWithRoadmapsProgress()
	{
		var id = User.GetId();

		var user = await userService.GetUserWithRoadmaps(id);

		return Ok(user);
	}
}
