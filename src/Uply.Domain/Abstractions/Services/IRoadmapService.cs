using Uply.Domain.Entities;
using Uply.Domain.Models.Dto.Roadmaps;
using Uply.Web.Models._Roadmap_;

namespace Uply.Domain.Abstractions.Services;

public interface IRoadmapService
{
    Task<RoadmapFullDto> CreateRoadmapAsync(CreateRoadmapModel createModel);
    Task<RoadmapFullDto> GetRoadmap(Guid roadmapId);
}
