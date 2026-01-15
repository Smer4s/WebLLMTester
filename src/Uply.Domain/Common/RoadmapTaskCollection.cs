using Microsoft.IdentityModel.Abstractions;
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

    public void InsertTask(RoadmapTask taskToInsert)
    {
        _taskCollection.Add(taskToInsert);

        MoveTask(taskToInsert.Id, taskToInsert.TaskNumber - 1);

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

    public RoadmapTask? CurrentTask
    {
        get
        {
            var currentTask = _taskCollection
                .Where(x => x.IsCompleted is false)
                .OrderBy(x => x.TaskNumber)
                .FirstOrDefault()
                ?? _taskCollection.FirstOrDefault();

            if (currentTask is not null)
            {
                currentTask.IsActiveTask = true;
            }

            return currentTask;
        }
    }

    public RoadmapTask? NextTask => CurrentTask is not null
        ? _taskCollection.FirstOrDefault(x => x.TaskNumber == CurrentTask.TaskNumber + 1)
        : null;

    public List<RoadmapTask> OrderedTasks => _taskCollection;

    public int TotalTaskCount => _taskCollection.Count;
    public int CompletedCount => _taskCollection.Count(x => x.IsCompleted);

    public int CompletedPercent => CompletedCount is 0
        ? 0
        : ((TotalTaskCount * 100) / (CompletedCount * 100)) * 100;
}
