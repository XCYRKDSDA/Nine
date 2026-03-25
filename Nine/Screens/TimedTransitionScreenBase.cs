using Microsoft.Xna.Framework;
using Nine.Screens;

/// <summary>
/// 定时的过渡界面基类. 其在指定固定时间后完成过渡
/// </summary>
public abstract class TimedTransitionScreenBase(
    ScreenManager screenManager,
    IScreen prevScreen,
    IScreen nextScreen,
    TimeSpan duration
) : TransitionScreenBase(screenManager, prevScreen, nextScreen)
{
    private TimeSpan _elapsedTime = TimeSpan.Zero;

    public TimeSpan Duration => duration;

    public TimeSpan ElapsedTime => _elapsedTime;

    private float _progress = 0;

    public float Progress => _progress;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _elapsedTime += gameTime.ElapsedGameTime;
        if (_elapsedTime > duration)
            ScreenManager.ActiveScreen = NextScreen;
    }
}
