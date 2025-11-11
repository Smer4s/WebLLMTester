using Bogus;
using MapsterMapper;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Entities;
using Uply.Domain.Enums;
using Uply.Domain.Models.Dto.Roadmaps;
using Uply.Web.Models._Roadmap_;

namespace Uply.Domain.Services;

public class RoadmapService(
    IUserRepository userRepository,
    IRoadmapRepository roadmapRepository,
    IMapper mapper) : IRoadmapService
{
    private const int DefaultTaskAmount = 30;

    public async Task<RoadmapFullDto> CreateRoadmapAsync(CreateRoadmapModel createRoadmapModel)
    {
        if (await userRepository.IsExistsAsync(createRoadmapModel.IssuerId) is false)
        {
            throw new Exception("Not existing user");
        }

        var tasks = GetTasks(createRoadmapModel);

        var roadmap = new Roadmap(tasks)
        {
            IssuerId = createRoadmapModel.IssuerId,
            Deadline = createRoadmapModel.Deadline,
            Goal = createRoadmapModel.Goal,
            ManHoursPerTask = createRoadmapModel.ManHoursPerTask,
            Period = createRoadmapModel.Period,
            StartingPoint = createRoadmapModel.StartingPoint,
        };

        await roadmapRepository.CreateAsync(roadmap);

        return mapper.Map<RoadmapFullDto>(roadmap);

    }

    public List<RoadmapTask> GetTasks(CreateRoadmapModel createRoadmapModel)
    {
        int taskAmount = DefaultTaskAmount;
        if (createRoadmapModel.Deadline.HasValue)
        {
            var daysRemain = (int)(createRoadmapModel.Deadline.Value.ToDateTime(TimeOnly.MinValue) - DateTime.Today).TotalDays;

            taskAmount = daysRemain / createRoadmapModel.Period.ToDaysCount();
        }

        var faker = new Faker<RoadmapTask>()
            .Rules((f, t) =>
            {
                t.Description = f.Lorem.Sentence(range: 10);
                t.Title = f.Lorem.Word();
            });

        var tasks = faker.Generate(taskAmount);

        for (int taskNumber = 1; taskNumber <= tasks.Count; taskNumber++)
        {
            var task = tasks[taskNumber - 1];
            task.TaskNumber = taskNumber;
        }

        return tasks;
    }
}
