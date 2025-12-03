using Microsoft.Xna.Framework;

namespace Nine.Screens;

public abstract class ScreenBase : IScreen
{
    protected ScreenBase(ScreenManager screenManager)
    {
        ScreenManager = screenManager;
    }

    public ScreenManager ScreenManager { get; private set; }

    public virtual void OnActivated() { }
    public virtual void OnDeactivated() { }

    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime);

    public virtual TransitionState TransitionState { get; set; }

    protected virtual void OnStartTransitIn(object? context) { }

    void IScreen.OnStartTransitIn(object? context)
    {
        TransitionState = TransitionState.TransitingIn;
        OnStartTransitIn(context);
    }

    protected virtual void OnFinishTransitIn(object? context) { }

    void IScreen.OnFinishTransitIn(object? context)
    {
        TransitionState = TransitionState.None;
        OnFinishTransitIn(context);
    }

    public virtual void OnTransitIn(object? context, float progress) { }


    protected virtual void OnStartTransitOut(object? context) { }

    void IScreen.OnStartTransitOut(object? context)
    {
        TransitionState = TransitionState.TransitingOut;
        OnStartTransitOut(context);
    }

    protected virtual void OnFinishTransitOut(object? context) { }

    void IScreen.OnFinishTransitOut(object? context)
    {
        TransitionState = TransitionState.None;
        OnFinishTransitOut(context);
    }

    public virtual void OnTransitOut(object? context, float progress) { }
}
