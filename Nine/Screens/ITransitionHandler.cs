namespace Nine.Screens;

public interface ITransitionHandler<TTransition>
{
    void OnStartTransition() { }

    void OnFinishTransition() { }
}
