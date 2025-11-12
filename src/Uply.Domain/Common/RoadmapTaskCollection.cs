using Uply.Domain.Constants;
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

    private bool IsRoadmapFull => _taskCollection.Count == RoadmapConstants.MaxTasksInRoadmap;

    public void InsertTask(RoadmapTask taskToInsert)
    {
        if (IsRoadmapFull) 
        {
            throw new InvalidOperationException($"Невозможно добавить новую задачу. Роадмап уже заполнен. Максимальное число задач:{RoadmapConstants.MaxTasksInRoadmap}");
        }

        _taskCollection.Add(taskToInsert);

        MoveTask(taskToInsert.Id, taskToInsert.TaskNumber);

        ReorderTasks();
    }

    //Индексация с 0
    public void MoveTask(Guid taskIdToMove, int newPosition)
    {
        var taskToMove = _taskCollection.FirstOrDefault(x => x.Id == taskIdToMove);

        if (taskToMove is null)
        {
            throw new InvalidOperationException("Задача отсутствует в коллекции.");
        }

        if (newPosition < 0 || newPosition > _taskCollection.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(newPosition),
                $"Позиция должна быть в диапазоне от 1 до {_taskCollection.Count}.");
        }

        _taskCollection.Remove(taskToMove);

        _taskCollection.Insert(newPosition, taskToMove);

        ReorderTasks();
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
