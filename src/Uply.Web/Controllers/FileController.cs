using Microsoft.AspNetCore.Mvc;
using Uply.Domain.Abstractions.Services;
using Uply.Web.Controllers.Abstract;

namespace Uply.Web.Controllers;

public class FileController(IMinioService minioService) : RestApiController
{
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUploadingFileUrl(string mimeType)
    {
        var result = await minioService.GetSignedUrlForUploadingFile(mimeType, true);

        return Ok(result);
    }
}