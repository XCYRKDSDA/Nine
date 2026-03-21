using Microsoft.Xna.Framework;
using Nine.Screens;

/// <summary>
/// 定时的过渡界面基类. 其在指定固定时间后完成过渡
/// </summary>
/// <typeparam name="TState"><inheritdoc cref="StatefulTransitionScreenBase{TState}"/></typeparam>
public abstract class StatefulTimedTransitionScreenBase<TState>(
    ScreenManager screenManager,
    IConfigurableScreen<TState> prevScreen,
    IConfigurableScreen<TState> nextScreen,
    TimeSpan duration
) : StatefulTransitionScreenBase<TState>(screenManager, prevScreen, nextScreen)
    where TState : struct
{
    private TimeSpan _elapsedTime = TimeSpan.Zero;

    public TimeSpan Duration => duration;

    public TimeSpan ElapsedTime => _elapsedTime;

    protected sealed override float UpdateProgress(GameTime gameTime)
    {
        _elapsedTime += gameTime.ElapsedGameTime;
        return (float)(_elapsedTime / duration);
    }
}
