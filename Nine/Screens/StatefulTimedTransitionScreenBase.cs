using Microsoft.Xna.Framework;
using Nine.Screens;

/// <summary>
/// 定时的过渡界面基类. 其在指定固定时间后完成过渡
/// </summary>
/// <typeparam name="TTransition"><inheritdoc cref="StatefulTransitionScreenBase{TTransition}"/></typeparam>
/// <typeparam name="TSourceState"><inheritdoc cref="StatefulTransitionScreenBase{TSourceState}"/></typeparam>
/// <typeparam name="TTargetState"><inheritdoc cref="StatefulTransitionScreenBase{TTargetState}"/></typeparam>
public abstract class StatefulTimedTransitionScreenBase<TTransition, TSourceState, TTargetState>(
    ScreenManager screenManager,
    ITransitionSourceScreen<TTransition, TSourceState> prevScreen,
    ITransitionTargetScreen<TTransition, TTargetState> nextScreen,
    TimeSpan duration
)
    : StatefulTransitionScreenBase<TTransition, TSourceState, TTargetState>(
        screenManager,
        prevScreen,
        nextScreen
    )
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
