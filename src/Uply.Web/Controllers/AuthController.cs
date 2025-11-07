using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Services;
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

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromQuery] string refreshToken)
    {
        var result = await authService.RefreshAsync(refreshToken);

        return Ok(result);
    }
}
