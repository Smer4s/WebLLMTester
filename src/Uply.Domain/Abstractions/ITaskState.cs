namespace Uply.Domain.Abstractions;

public interface ITaskState
{
    bool IsCompleted { get; }
}
