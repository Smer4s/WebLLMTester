using Microsoft.AspNetCore.Mvc;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Models.Dto.RoadmapTasks.Commands;
using Uply.Web.Controllers.Abstract;

namespace Uply.Web.Controllers;

public class RoadmapTaskController(IRoadmapTaskService roadmapTaskService) : RestApiController
{
    [HttpPut]
    public async Task<IActionResult> UpdateRoadmapTask(UpdateRoadmapTaskDto dto)
    {
        var updatedDto = await roadmapTaskService.UpdateRoadmapTask(dto);

        return Ok(updatedDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoadmapTask(CreateRoadmapTaskDto dto)
    {
        await roadmapTaskService.CreateRoadmapTask(dto);

        return Ok();
    }

    [HttpPut("moveTask")]
    public async Task<IActionResult> MoveRoadmapTask(MoveRoadmapTaskDto dto)
    {
        await roadmapTaskService.MoveRoadmapTask(dto);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> UpdateRoadmapTask([FromRoute] Guid id)
    {
        await roadmapTaskService.DeleteRoadmapTask(id);

        return Ok();
    }
}
