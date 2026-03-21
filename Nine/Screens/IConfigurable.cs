namespace Nine.Screens;

public interface IConfigurable<T>
{
    void ApplyState(in T state);
}
