namespace Nine.Screens;

public interface ITaskLike<out T>
    where T : IScreen
{
    public AggregateException? Exception { get; }
    public bool IsCompleted { get; }
    public bool IsCanceled { get; }
    public bool IsCompletedSuccessfully { get; }
    public bool IsFaulted { get; }
    public TaskStatus Status { get; }

    public T Result { get; }
}
