using Uply.Domain.Common;
using Uply.Domain.Entities;

namespace Uply.Tests;

[TestClass]
public class RoadmapCollectionTests
{
    private List<RoadmapTask> CreateInitialTasks(int count)
    {
        var tasks = new List<RoadmapTask>();
        for (int i = 1; i <= count; i++)
        {
            tasks.Add(new RoadmapTask
            {
                Id = Guid.NewGuid(),
                Title = $"Task {i}",
                Description = $"Description {i}",
                TaskNumber = i
            });
        }
        return tasks;
    }

    [TestMethod]
    public void InsertTask_AtEnd_ShouldIncreaseCountAndSetCorrectTaskNumber()
    {
        var tasks = CreateInitialTasks(3);
        var collection = new RoadmapTaskCollection(tasks);

        var newTask = new RoadmapTask
        {
            Id = Guid.NewGuid(),
            Title = "New End Task",
            Description = "Inserted at end",
            TaskNumber = 4
        };

        collection.InsertTask(newTask);

        Assert.AreEqual(4, collection.OrderedTasks.Count);
        Assert.AreEqual(4, newTask.TaskNumber);
        Assert.AreEqual(newTask, collection.OrderedTasks.Last());
    }

    [TestMethod]
    public void InsertTask_AtMiddle_ShouldIncreaseCountAndReorder()
    {
        var tasks = CreateInitialTasks(4);
        var collection = new RoadmapTaskCollection(tasks);

        var newTask = new RoadmapTask
        {
            Id = Guid.NewGuid(),
            Title = "Middle Task",
            Description = "Inserted in middle",
            TaskNumber = 2
        };

        collection.InsertTask(newTask);

        Assert.AreEqual(5, collection.OrderedTasks.Count);
        Assert.AreEqual(2, newTask.TaskNumber);
        Assert.AreEqual(newTask.Id, collection.OrderedTasks[1].Id);
        // Проверим, что остальные задачи сдвинулись
        Assert.AreEqual(3, collection.OrderedTasks[2].TaskNumber);
    }

    [TestMethod]
    public void InsertTask_AtBeginning_ShouldIncreaseCountAndReorder()
    {
        var tasks = CreateInitialTasks(3);
        var collection = new RoadmapTaskCollection(tasks);

        var newTask = new RoadmapTask
        {
            Id = Guid.NewGuid(),
            Title = "First Task",
            Description = "Inserted at beginning",
            TaskNumber = 1
        };

        collection.InsertTask(newTask);

        Assert.AreEqual(4, collection.OrderedTasks.Count);
        Assert.AreEqual(1, newTask.TaskNumber);
        Assert.AreEqual(newTask.Id, collection.OrderedTasks.First().Id);
    }

    [TestMethod]
    public void RemoveTask_AtEnd_ShouldDecreaseCountAndReorder()
    {
        var tasks = CreateInitialTasks(3);
        var collection = new RoadmapTaskCollection(tasks);

        var lastTask = collection.OrderedTasks.Last();
        collection.RemoveTask(lastTask);

        Assert.AreEqual(2, collection.OrderedTasks.Count);
        Assert.IsFalse(collection.OrderedTasks.Any(t => t.Id == lastTask.Id));
        Assert.AreEqual(2, collection.OrderedTasks.Last().TaskNumber);
    }

    [TestMethod]
    public void RemoveTask_AtBeginning_ShouldDecreaseCountAndReorder()
    {
        var tasks = CreateInitialTasks(3);
        var collection = new RoadmapTaskCollection(tasks);

        var firstTask = collection.OrderedTasks.First();
        collection.RemoveTask(firstTask);

        Assert.AreEqual(2, collection.OrderedTasks.Count);
        Assert.IsFalse(collection.OrderedTasks.Any(t => t.Id == firstTask.Id));
        Assert.AreEqual(1, collection.OrderedTasks.First().TaskNumber);
    }

    [TestMethod]
    public void RemoveTask_AtMiddle_ShouldDecreaseCountAndReorder()
    {
        var tasks = CreateInitialTasks(4);
        var collection = new RoadmapTaskCollection(tasks);

        var middleTask = collection.OrderedTasks[1]; // TaskNumber = 2
        collection.RemoveTask(middleTask);

        Assert.AreEqual(3, collection.OrderedTasks.Count);
        Assert.IsFalse(collection.OrderedTasks.Any(t => t.Id == middleTask.Id));
        // Проверим, что задачи перенумеровались
        Assert.AreEqual(2, collection.OrderedTasks[1].TaskNumber);
    }

    [TestMethod]
    public void MoveTask_ToBeginning_ShouldReorderCorrectly()
    {
        var tasks = CreateInitialTasks(3);
        var collection = new RoadmapTaskCollection(tasks);

        var taskToMove = collection.OrderedTasks[2]; // TaskNumber = 3
        collection.MoveTask(taskToMove.Id, 0); // перемещаем в начало

        Assert.AreEqual(taskToMove.Id, collection.OrderedTasks.First().Id);
        Assert.AreEqual(1, collection.OrderedTasks.First().TaskNumber);
        Assert.AreEqual(3, collection.OrderedTasks.Count);
    }

    [TestMethod]
    public void MoveTask_ToMiddle_ShouldReorderCorrectly()
    {
        var tasks = CreateInitialTasks(4);
        var collection = new RoadmapTaskCollection(tasks);

        var taskToMove = collection.OrderedTasks[0]; // TaskNumber = 1
        collection.MoveTask(taskToMove.Id, 2); // перемещаем на позицию 2 (индексация с 0)

        Assert.AreEqual(taskToMove.Id, collection.OrderedTasks[2].Id);
        Assert.AreEqual(3, collection.OrderedTasks[2].TaskNumber);
        Assert.AreEqual(4, collection.OrderedTasks.Count);
    }

    [TestMethod]
    public void MoveTask_ToEnd_ShouldReorderCorrectly()
    {
        var tasks = CreateInitialTasks(3);
        var collection = new RoadmapTaskCollection(tasks);

        var taskToMove = collection.OrderedTasks[0]; // TaskNumber = 1
        collection.MoveTask(taskToMove.Id, collection.OrderedTasks.Count - 1); // перемещаем в конец

        Assert.AreEqual(taskToMove.Id, collection.OrderedTasks.Last().Id);
        Assert.AreEqual(3, collection.OrderedTasks.Last().TaskNumber);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void MoveTask_TaskNotFound_ShouldThrow()
    {
        var tasks = CreateInitialTasks(2);
        var collection = new RoadmapTaskCollection(tasks);

        collection.MoveTask(Guid.NewGuid(), 1); // несуществующая задача
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void MoveTask_InvalidPosition_ShouldThrow()
    {
        var tasks = CreateInitialTasks(2);
        var collection = new RoadmapTaskCollection(tasks);

        var taskToMove = collection.OrderedTasks[0];
        collection.MoveTask(taskToMove.Id, 10); // позиция вне диапазона
    }
}
