using Microsoft.Xna.Framework;

namespace Nine.Screens;

/// <summary>
/// 过渡界面基类
/// </summary>
/// <typeparam name="TState">过渡状态类型. 常用于控制前后界面过渡动画</typeparam>
public abstract class StatefulTransitionScreenBase<TState>(
    ScreenManager screenManager,
    ITransitionSourceScreen<TState> prevScreen,
    ITransitionTargetScreen<TState> nextScreen
) : ScreenBase(screenManager)
{
    public ITransitionSourceScreen<TState> PrevScreen => prevScreen;

    public ITransitionTargetScreen<TState> NextScreen => nextScreen;

    private float _progress = 0;

    public float Progress => _progress;

    protected abstract float UpdateProgress(GameTime gameTime);

    protected abstract TState InterpolateTransitionState(
        TState source,
        TState target,
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

        // 获取前后界面源状态和目标状态
        var sourceState = prevScreen.GetSourceTransitionState();
        var targetState = nextScreen.GetTargetTransitionState();

        // 计算过渡进度
        _progress = UpdateProgress(gameTime);
        if (_progress >= 1)
        {
            // 完成过渡, 切换活跃界面
            ScreenManager.ActiveScreen = NextScreen;
            _progress = 1;
        }

        // 插值得到当前过渡状态
        var currentState = InterpolateTransitionState(sourceState, targetState, _progress);

        // 应用过渡状态, 控制前后界面的过渡效果
        prevScreen.ApplyState(in currentState);
        nextScreen.ApplyState(in currentState);
    }
}
