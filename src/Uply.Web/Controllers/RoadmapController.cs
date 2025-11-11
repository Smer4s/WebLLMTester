using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uply.Domain.Abstractions.Services;
using Uply.Web.Controllers.Abstract;
using Uply.Web.Extensions;
using Uply.Web.Models._Roadmap_;

namespace Uply.Web.Controllers;

public class RoadmapController(IRoadmapService roadmapService) : RestApiController
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateRoadmap(CreateRoadmapRequest model)
    {
        var userId = User.GetId();

        var createModel = new CreateRoadmapModel()
        {
            IssuerId = userId,
            Deadline = model.Deadline,
            Goal = model.Goal,
            ManHoursPerTask = model.ManHoursPerTask,
            Period = model.Period,
            StartingPoint = model.StartingPoint,
        };

        var roadmap = await roadmapService.CreateRoadmapAsync(createModel);

        return Ok(roadmap);
    }
}
