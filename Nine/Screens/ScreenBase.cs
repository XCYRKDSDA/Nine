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

    protected virtual void OnStartTransitIn() { }

    void IScreen.OnStartTransitIn()
    {
        TransitionState = TransitionState.TransitingIn;
        OnStartTransitIn();
    }

    public virtual void OnTransitIn(float progress) { }

    protected virtual void OnStartTransitOut() { }

    void IScreen.OnStartTransitOut()
    {
        TransitionState = TransitionState.TransitingOut;
        OnStartTransitOut();
    }

    public virtual void OnTransitOut(float progress) { }
}
