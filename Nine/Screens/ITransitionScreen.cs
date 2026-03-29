namespace Nine.Screens;

public interface ITransitionScreen : IScreen
{
    IScreen PrevScreen { get; }

    IScreen? NextScreen { get; }

    event EventHandler? TransitionDone;
}
