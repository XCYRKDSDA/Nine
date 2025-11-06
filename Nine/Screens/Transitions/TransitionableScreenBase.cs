namespace Nine.Screens.Transitions;

public enum TransitionState
{
    None,
    TransitingIn,
    TransitingOut,
}

public abstract class TransitionableScreenBase(ScreenManager screenManager)
    : ScreenBase(screenManager), ITransitionableScreen
{
    public virtual TransitionState TransitionState { get; set; }

    protected virtual void OnStartTransitIn() { }

    void ITransitionableScreen.OnStartTransitIn()
    {
        TransitionState = TransitionState.TransitingIn;
        OnStartTransitIn();
    }

    public virtual void OnTransitIn(float progress) { }

    protected virtual void OnStartTransitOut() { }

    void ITransitionableScreen.OnStartTransitOut()
    {
        TransitionState = TransitionState.TransitingOut;
        OnStartTransitOut();
    }

    public virtual void OnTransitOut(float progress) { }
}
