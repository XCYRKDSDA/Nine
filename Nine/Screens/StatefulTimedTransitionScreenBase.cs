using Microsoft.Xna.Framework;
using Nine.Screens;

/// <summary>
/// 定时的过渡界面基类. 其在指定固定时间后完成过渡
/// </summary>
/// <typeparam name="TSourceState"><inheritdoc cref="StatefulTransitionScreenBase{TSourceState}"/></typeparam>
/// <typeparam name="TTargetState"><inheritdoc cref="StatefulTransitionScreenBase{TTargetState}"/></typeparam>
public abstract class StatefulTimedTransitionScreenBase<TSourceState, TTargetState>(
    IVisualConfigurableScreen<TSourceState> prevScreen,
    IVisualConfigurableScreen<TTargetState> nextScreen,
    TimeSpan duration
) : StatefulTransitionScreenBase<TSourceState, TTargetState>(prevScreen, nextScreen)
{
    private TimeSpan _elapsedTime = TimeSpan.Zero;

    public TimeSpan Duration => duration;

    public TimeSpan ElapsedTime => _elapsedTime;

    private float _progress = 0;

    public float Progress => _progress;

    public override void Update(GameTime gameTime)
    {
        _elapsedTime += gameTime.ElapsedGameTime;
        _progress = (float)(_elapsedTime / duration);
        TransitionState = TransitionState.InProgress;

        if (_elapsedTime > duration)
        {
            _progress = 1;
            TransitionState = TransitionState.Completed;
        }

        base.Update(gameTime);
    }

    public override void UpdateBackward(GameTime gameTime)
    {
        _elapsedTime -= gameTime.ElapsedGameTime;
        _progress = (float)(_elapsedTime / duration);
        TransitionState = TransitionState.InProgress;

        if (_elapsedTime <= TimeSpan.Zero)
        {
            _progress = 0;
            TransitionState = TransitionState.Pending;
        }

        base.Update(gameTime);
    }
}
