using Uply.Domain.Entities;
using Uply.Web.Models._Roadmap_;

namespace Uply.Domain.Abstractions.Services;

public interface IChatGptClient
{
    Task<List<RoadmapTask>> GenerateRoadmapTasksAsync(CreateRoadmapModel model);
}
