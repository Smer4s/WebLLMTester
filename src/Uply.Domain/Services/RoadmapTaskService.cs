using MapsterMapper;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Models.Dto.RoadmapTasks;
using Uply.Domain.Models.Dto.RoadmapTasks.Commands;

namespace Uply.Domain.Services;

public class RoadmapTaskService(
    IRoadmapTaskRepository roadmapTaskRepository,
    IRoadmapRepository roadmapRepository,
    IMapper mapper)
    : IRoadmapTaskService
{
    public async Task DeleteRoadmapTask(Guid roadmapTaskId)
    {
        var taskToDelete = await roadmapTaskRepository.GetByIdWithIncludes(roadmapTaskId);
        if (taskToDelete is null)
        {
            throw new ArgumentNullException("Задача не была найдена");
        }

        var roadmap = taskToDelete.Roadmap;
        var taskCollection = roadmap.RoadmapTasks;
        taskCollection.RemoveTask(taskToDelete);

        await roadmapTaskRepository.DeleteAsync(taskToDelete.Id);
        await roadmapRepository.UpdateAsync(roadmap);
    }

    public async Task<RoadmapTaskDto> UpdateRoadmapTask(UpdateRoadmapTaskDto updateDto)
    {
        var taskToUpdate = await roadmapTaskRepository.GetByIdAsync(updateDto.Id);
        if (taskToUpdate is null)
        {
            throw new ArgumentNullException("Задача не была найдена");
        }

        taskToUpdate.Description = updateDto.Description;
        taskToUpdate.Title = updateDto.Title;

        await roadmapTaskRepository.UpdateAsync(taskToUpdate);

        return mapper.Map<RoadmapTaskDto>(taskToUpdate);
    }
}
