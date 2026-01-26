using Microsoft.AspNetCore.Mvc;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Models._Skins_;
using Uply.Domain.Models.Dto.Skin;
using Uply.Web.Controllers.Abstract;

namespace Uply.Web.Controllers;

public class SkinController(ISkinsService skinsService) : RestApiController
{
    [HttpGet("skins")]
    [ProducesResponseType(typeof(IEnumerable<SkinDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSkins()
    {
        var data = await skinsService.GetSkins();
        return Ok(data);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SkinDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateSkin([FromBody] CreateSkinDto skin)
    {
        var result = await skinsService.CreateSkin(skin);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SkinDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSkin(Guid id)
    {
        var result = await skinsService.GetSkin(id);
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(SkinDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSkin([FromBody] UpdateSkinDto skin)
    {
        var result = await skinsService.UpdateSkin(skin);
        return Ok(result);
    }
}