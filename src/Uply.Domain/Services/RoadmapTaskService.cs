using MapsterMapper;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Entities;
using Uply.Domain.Models.Dto.RoadmapTasks;
using Uply.Domain.Models.Dto.RoadmapTasks.Commands;

namespace Uply.Domain.Services;

public class RoadmapTaskService(
    IRoadmapTaskRepository roadmapTaskRepository,
    IRoadmapRepository roadmapRepository,
    IMapper mapper)
    : IRoadmapTaskService
{
    public async Task CreateRoadmapTask(CreateRoadmapTaskDto createRoadmapDto)
    {
        var roadmap = await roadmapRepository.GetRoadmapByIdAsyncWithIncludes(createRoadmapDto.RoadmapId);

        if (roadmap == null) 
        {
            throw new ArgumentNullException("Роудмап не был найден");
        }

        var newTask = new RoadmapTask()
        { 
            Id = Guid.NewGuid(),
            Title = createRoadmapDto.Title,
            Description = createRoadmapDto.Description,
            TaskNumber = createRoadmapDto.TaskNumber,
            Roadmap = roadmap,
            RoadmapId = roadmap.Id,
        };

        roadmap.RoadmapTasks.InsertTask(newTask);
        await roadmapRepository.UpdateAsync(roadmap);
        await roadmapTaskRepository.CreateAsync(newTask);
    }

    public async Task MoveRoadmapTask(MoveRoadmapTaskDto moveDto)
    {
        var roadmap = await roadmapRepository.GetRoadmapByIdAsyncWithIncludes(moveDto.RoadmapId);

        if (roadmap == null)
        {
            throw new ArgumentNullException("Роудмап не был найден");
        }

        roadmap.RoadmapTasks.MoveTask(moveDto.RoadmapTaskId, moveDto.NewPosition);
        await roadmapRepository.UpdateAsync(roadmap);
    }

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
        var taskToUpdate = await roadmapTaskRepository.GetByIdWithIncludes(updateDto.Id);
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
