using Microsoft.Xna.Framework;

namespace Nine.Screens.Transitions;

public class CustomTransition(
    ScreenManager screenManager, ITransitionableScreen prevScreen, ITransitionableScreen nextScreen, TimeSpan duration)
    : TransitionBase(screenManager, prevScreen, nextScreen)
{
    private TimeSpan _duration = TimeSpan.Zero;

    public float Progress => (float)(_duration / duration);

    public override void OnActivated()
    {
        base.OnActivated();
        prevScreen.OnStartTransitOut();
        nextScreen.OnStartTransitIn();
    }

    public override void Update(GameTime gameTime)
    {
        _duration += gameTime.ElapsedGameTime;
        var progress = (float)(_duration / duration);
        prevScreen.OnTransitOut(progress);
        nextScreen.OnTransitIn(progress);

        if (_duration >= duration)
            ScreenManager.ActiveScreen = nextScreen;
    }

    public override void Draw(GameTime gameTime)
    {
        prevScreen.Draw(gameTime);
        nextScreen.Draw(gameTime);
    }
}
