namespace Nine.Screens;

public interface IScreenFactory
{
    IScreen CreateScreen(Type screenType, object? arguments = null);

    ITransitionScreen CreateTransitionScreen(
        Type transitionScreenType,
        IScreen prevScreen,
        IScreen nextScreen,
        object? arguments = null
    );
}
