using Uply.Domain.Entities;
using Uply.Web.Models._TaskReport_;

namespace Uply.Domain.Abstractions.Services;

public interface ITaskReportService
{
    Task<TaskReport> CreateReportAsync(CreateRoadmapReportDto dto);
    Task<TaskReport> UpdateReportAsync(Guid reportId, string? description, ICollection<string> photoUrls);
}
