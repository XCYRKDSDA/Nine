namespace Nine.Screens.Transitions;

public interface ITransitionableScreen : IScreen
{
    void OnStartTransitIn();

    void OnTransitIn(float progress);

    void OnStartTransitOut();

    void OnTransitOut(float progress);
}
