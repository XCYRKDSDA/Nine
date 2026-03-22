using Microsoft.Xna.Framework;

namespace Nine.Screens;

/// <summary>
/// 过渡界面基类
/// </summary>
/// <typeparam name="TTransition">过渡标签类型</typeparam>
/// <typeparam name="TSourceState">源界面的过渡视觉状态类型</typeparam>
/// <typeparam name="TTargetState">目标界面的过渡视觉状态类型</typeparam>
public abstract class StatefulTransitionScreenBase<TTransition, TSourceState, TTargetState>(
    ScreenManager screenManager,
    ITransitionSourceScreen<TTransition, TSourceState> prevScreen,
    ITransitionTargetScreen<TTransition, TTargetState> nextScreen
) : ScreenBase(screenManager)
{
    public ITransitableScreen<TTransition, TSourceState> PrevScreen => prevScreen;

    public ITransitableScreen<TTransition, TTargetState> NextScreen => nextScreen;

    private float _progress = 0;

    public float Progress => _progress;

    protected abstract float UpdateProgress(GameTime gameTime);

    protected abstract (
        TSourceState SourceState,
        TTargetState TargetState
    ) InterpolateTransitionState(
        TSourceState? sourceConstraint,
        TTargetState? targetConstraint,
        float progress
    );

    public override void OnActivated()
    {
        base.OnActivated();
        prevScreen.OnStartTransition();
        nextScreen.OnStartTransition();
    }

    public override void OnDeactivated()
    {
        base.OnDeactivated();
        prevScreen.OnFinishTransition();
        nextScreen.OnFinishTransition();
    }

    public override void Update(GameTime gameTime)
    {
        // 前后界面各自照常更新
        prevScreen.Update(gameTime);
        nextScreen.Update(gameTime);

        // 计算过渡进度
        _progress = UpdateProgress(gameTime);
        if (_progress >= 1)
        {
            // 完成过渡, 切换活跃界面
            ScreenManager.ActiveScreen = NextScreen;
            _progress = 1;
        }

        // 插值得到当前过渡状态
        var sourceConstraint = prevScreen.GetTransitionSourceConstraint();
        var targetConstraint = nextScreen.GetTransitionTargetConstraint();
        var (sourceState, targetState) = InterpolateTransitionState(
            sourceConstraint,
            targetConstraint,
            _progress
        );

        // 应用过渡状态, 控制前后界面的过渡效果
        prevScreen.ApplyState(in sourceState);
        nextScreen.ApplyState(in targetState);
    }
}
