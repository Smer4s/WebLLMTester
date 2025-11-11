using Uply.Domain.Entities;

namespace Uply.Domain.Common;

public class RoadmapTaskCollection
{
    private readonly List<RoadmapTask> _taskCollection;

    private void ReorderTasks()
    {
        for (int taskNumber = 1; taskNumber <= _taskCollection.Count; taskNumber++)
        {
            var task = _taskCollection[taskNumber - 1];
            task.TaskNumber = taskNumber;
        }
    }

    public RoadmapTaskCollection(ICollection<RoadmapTask> tasks)
    {
        for (int taskNumber = 1; taskNumber <= tasks.Count; taskNumber++)
        {
            var task = tasks.Single(x => x.TaskNumber == taskNumber);
        }

        _taskCollection = tasks.OrderBy(x => x.TaskNumber).ToList();
    }

    public void RemoveTask(RoadmapTask taskToRemove)
    {
        _taskCollection.Remove(taskToRemove);
        ReorderTasks();
    }

    public RoadmapTask? CurrentTask => _taskCollection
        .Where(x => x.IsCompleted is false)
        .OrderBy(x => x.TaskNumber)
        .FirstOrDefault();

    public RoadmapTask? NextTask => CurrentTask is not null
        ? _taskCollection.FirstOrDefault(x => x.TaskNumber == 1 + CurrentTask.TaskNumber)
        : null;

    public List<RoadmapTask> OrderedTasks => _taskCollection;
}
