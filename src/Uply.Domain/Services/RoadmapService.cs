using Bogus;
using MapsterMapper;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Constants;
using Uply.Domain.Entities;
using Uply.Domain.Enums;
using Uply.Domain.Models.Dto.Roadmaps;
using Uply.Web.Models._Roadmap_;

namespace Uply.Domain.Services;

public class RoadmapService(
	IUserRepository userRepository,
	IRoadmapRepository roadmapRepository,
	IChatGptClient chatGptClient,
	IMapper mapper) : IRoadmapService
{
	public async Task<RoadmapFullDto> GetRoadmap(Guid roadmapId)
	{
		var roadmap = await roadmapRepository.GetRoadmapByIdAsyncWithIncludes(roadmapId);

		if (roadmap == null)
		{
			throw new ArgumentNullException("Роудмап не был найден");
		}

		return mapper.Map<RoadmapFullDto>(roadmap);
	}

	public async Task<RoadmapProgressDto> GetProgressRoadmap(Guid roadmapId)
	{
		var roadmap = await roadmapRepository.GetRoadmapByIdAsyncWithIncludes(roadmapId);

		if (roadmap == null)
		{
			throw new ArgumentNullException("Роудмап не был найден");
		}

		return mapper.Map<RoadmapProgressDto>(roadmap);
	}

	public async Task<RoadmapFullDto> CreateRoadmapAsync(CreateRoadmapModel createRoadmapModel)
	{
		if (await userRepository.IsExistsAsync(createRoadmapModel.IssuerId) is false)
		{
			throw new Exception("Not existing user");
		}

		var tasks = await chatGptClient.GenerateRoadmapTasksAsync(createRoadmapModel);

		var roadmap = new Roadmap(tasks)
		{
			Title = createRoadmapModel.Title,
			IssuerId = createRoadmapModel.IssuerId,
			Deadline = createRoadmapModel.Deadline,
			Goal = createRoadmapModel.Goal,
			ManHoursPerTask = createRoadmapModel.ManHoursPerTask,
			Period = createRoadmapModel.Period,
			StartingPoint = createRoadmapModel.StartingPoint,
			Start = DateTime.UtcNow,
			LastRoadmapActivity = DateTime.UtcNow,
		};

		await roadmapRepository.CreateAsync(roadmap);

		return mapper.Map<RoadmapFullDto>(roadmap);

	}

	public List<RoadmapTask> GetTasks(CreateRoadmapModel createRoadmapModel)
	{
		int taskAmount = RoadmapConstants.MaxTasksInRoadmap;
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

	public async Task DeleteRoadmap(Guid roadmapId)
	{
		var roadMap = await roadmapRepository.GetByIdAsync(roadmapId);

		if (roadMap is null)
		{
			throw new Exception("Not existing roadMap");
		}

		await roadmapRepository.DeleteAsync(roadmapId);
	}
}
