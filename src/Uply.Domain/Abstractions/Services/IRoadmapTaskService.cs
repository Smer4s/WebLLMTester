using Uply.Domain.Entities;
using Uply.Domain.Models.Dto.RoadmapTasks;
using Uply.Domain.Models.Dto.RoadmapTasks.Commands;

namespace Uply.Domain.Abstractions.Services;

public interface IRoadmapTaskService
{
    Task<RoadmapTaskDto> UpdateRoadmapTask(UpdateRoadmapTaskDto updateDto);
    Task DeleteRoadmapTask(Guid roadmapTaskId);
    Task CreateRoadmapTask(CreateRoadmapTaskDto createRoadmapDto);
    Task MoveRoadmapTask(MoveRoadmapTaskDto moveDto);
}
