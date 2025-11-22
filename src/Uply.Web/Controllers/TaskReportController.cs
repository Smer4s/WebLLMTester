using Microsoft.AspNetCore.Mvc;
using System.Net;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Entities;
using Uply.Domain.Models.Dto.UserDtos;
using Uply.Web.Controllers.Abstract;
using Uply.Web.Models._TaskReport_;

namespace Uply.Web.Controllers;

public class TaskReportController(ITaskReportService taskReportService) : RestApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateReport(CreateRoadmapReportDto dto)
    {
        var report = await taskReportService.CreateReportAsync(dto);
        return Ok(report);
    }

    [HttpPut("{reportId:guid}")]
    [ProducesResponseType(typeof(TaskReport), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateReport(Guid reportId, [FromBody] CreateRoadmapReportDto dto)
    {
        var report = await taskReportService.UpdateReportAsync(reportId, dto.Description, dto.PhotoUrls);
        return Ok(report);
    }
}
