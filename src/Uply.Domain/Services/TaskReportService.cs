using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Entities;
using Uply.Web.Models._TaskReport_;

namespace Uply.Domain.Services;

public class TaskReportService(ITaskReportRepository reportRepository, IRoadmapTaskRepository taskRepository) : ITaskReportService
{
    public async Task<TaskReport> CreateReportAsync(CreateRoadmapReportDto dto)
    {
        var task = await taskRepository.GetByIdWithIncludes(dto.RoadmapTaskId) ?? throw new InvalidOperationException("Задача не найдена");

        var report = new TaskReport
        {
            Id = Guid.NewGuid(),
            Description = dto.Description,
            PhotoUrls = dto.PhotoUrls,
            Task = task
        };

        task.TaskReport = report;
        task.TaskReportId = report.Id;

        await reportRepository.CreateAsync(report);
        await taskRepository.UpdateAsync(task);

        return report;
    }

    public async Task<TaskReport> UpdateReportAsync(Guid reportId, string? description, ICollection<string> photoUrls)
    {
        var report = await reportRepository.GetByIdAsync(reportId) ?? throw new InvalidOperationException("Отчёт не найден");
        report.Description = description;
        report.PhotoUrls = photoUrls;

        await reportRepository.UpdateAsync(report);

        return report;
    }
}
