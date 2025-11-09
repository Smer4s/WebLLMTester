using Microsoft.AspNetCore.Mvc;

namespace Uply.Web.Controllers.Abstract;

[ApiController]
[Route("/[controller]")]
public abstract class RestApiController : ControllerBase;
