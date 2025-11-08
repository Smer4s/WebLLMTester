using Bogus;
using MapsterMapper;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Entities;
using Uply.Domain.Enums;
using Uply.Domain.Models.Dto;
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

        var roadmap = new Roadmap()
        {
            IssuerId = createRoadmapModel.IssuerId,
            Deadline = createRoadmapModel.Deadline,
            Goal = createRoadmapModel.Goal,
            ManHoursPerTask = createRoadmapModel.ManHoursPerTask,
            Period = createRoadmapModel.Period,
            StartingPoint = createRoadmapModel.StartingPoint,
            Tasks = tasks,
        };

        await roadmapRepository.CreateAsync(roadmap);

        return mapper.Map<RoadmapFullDto>(roadmap);

    }

    public ICollection<RoadmapTask> GetTasks(CreateRoadmapModel createRoadmapModel)
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
                t.IsCompleted = false;
                t.Description = f.Lorem.Sentence(range: 10);
                t.Title = f.Lorem.Word();
            });

        return faker.Generate(taskAmount);
    }
}
