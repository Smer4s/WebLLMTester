using Microsoft.AspNetCore.Mvc;
using Uply.Web.Controllers.Abstract;

namespace Uply.Web.Controllers;

public class AuthController : RestApiController
{
    [HttpGet]
    public async Task<IActionResult> Authorize()
    {
        await Task.Delay(500);
        return Ok();
    }
}
