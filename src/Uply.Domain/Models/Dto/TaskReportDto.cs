using Mapster;
using Uply.Domain.Entities;

namespace Uply.Domain.Models.Dto;

public record TaskReportDto : IMapFrom<TaskReport>
{
    public Guid Id { get; init; }
    public string? Description { get; init; }
    public ICollection<string> PhotoUrls { get; init; } = [];
}
