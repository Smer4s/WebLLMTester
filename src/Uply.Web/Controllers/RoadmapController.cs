using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Models.Dto;
using Uply.Domain.Models.Dto.Roadmaps;
using Uply.Web.Controllers.Abstract;
using Uply.Web.Extensions;
using Uply.Web.Models._Roadmap_;

namespace Uply.Web.Controllers;

public class RoadmapController(IRoadmapService roadmapService) : RestApiController
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(RoadmapFullDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateRoadmap(CreateRoadmapRequest model)
    {
        var userId = User.GetId();

        var createModel = new CreateRoadmapModel()
        {
            Title = model.Title,
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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RoadmapFullDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoadmap([FromRoute] Guid id)
    {
        var roadmap = await roadmapService.GetRoadmap(id);

        return Ok(roadmap);
    }
}
