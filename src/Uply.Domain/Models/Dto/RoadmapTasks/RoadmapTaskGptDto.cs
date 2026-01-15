using System.Text.Json.Serialization;
using Uply.Domain.Entities;

namespace Uply.Domain.Models.Dto.RoadmapTasks;

public record RoadmapTaskGptDto
{

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;
}
