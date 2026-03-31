namespace Nine.Screens;

public interface IScreenFactory
{
    IScreen CreateScreen(Type screenType, object? arguments = null);

    ITaskLike<IScreen> CreateScreen2(Type screenType, Task<object?> contextTask);

    ITransitionScreen CreateTransitionScreen(
        Type transitionScreenType,
        IScreen prevScreen,
        IScreen nextScreen,
        object? arguments = null
    );

    ITransitionScreen CreateTransitionScreen2(
        Type transitionScreenType,
        IScreen prevScreen,
        ITaskLike<IScreen> nextScreenTask,
        object? arguments = null
    );
}
