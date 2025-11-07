using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Abstractions.Services;
using Uply.Web.Controllers.Abstract;

namespace Uply.Web.Controllers;

public class AuthController(IAuthService authService) : RestApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Authorize([FromQuery] string initDataRaw)
    {
        var user = await authService.AuthorizeAsync(initDataRaw);

        return Ok(user);
    }
}
