using Uply.Domain.Abstractions;
using Uply.Domain.Entities.Abstract;
using System.Text.Json.Serialization;

namespace Uply.Domain.Entities;


public class RoadmapTask : BaseEntity, ITaskState
{
    public Guid RoadmapId { get; set; }

    [JsonIgnore] // навигационное свойство, в JSON его нет
    public Roadmap Roadmap { get; set; } = null!;

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonIgnore] // вычисляемое свойство
    public bool IsCompleted => TaskReportId.HasValue;

    public int TaskNumber { get; set; }

    [JsonIgnore] // навигационное свойство
    public TaskReport? TaskReport { get; set; }

    public Guid? TaskReportId { get; set; }
}
