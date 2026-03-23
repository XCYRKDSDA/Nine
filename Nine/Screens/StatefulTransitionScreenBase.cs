using Microsoft.Xna.Framework;

namespace Nine.Screens;

/// <summary>
/// 过渡界面基类
/// </summary>
/// <typeparam name="TSourceState">源界面的过渡视觉状态类型</typeparam>
/// <typeparam name="TTargetState">目标界面的过渡视觉状态类型</typeparam>
public abstract class StatefulTransitionScreenBase<TSourceState, TTargetState>(
    ScreenManager screenManager,
    IVisualConfigurableScreen<TSourceState> prevScreen,
    IVisualConfigurableScreen<TTargetState> nextScreen
) : ScreenBase(screenManager)
{
    public IVisualConfigurableScreen<TSourceState> PrevScreen => prevScreen;

    public IVisualConfigurableScreen<TTargetState> NextScreen => nextScreen;

    private float _progress = 0;

    public float Progress => _progress;

    protected abstract float UpdateProgress(GameTime gameTime);

    protected abstract (TSourceState SourceState, TTargetState TargetState) InterpolateVisualState(
        TSourceState? sourceDefaultState,
        TTargetState? targetDefaultState,
        float progress
    );

    public override void OnActivated()
    {
        base.OnActivated();
        prevScreen.EnterConfigurationMode();
        nextScreen.EnterConfigurationMode();
    }

    public override void OnDeactivated()
    {
        base.OnDeactivated();
        prevScreen.ExitConfigurationMode();
        nextScreen.ExitConfigurationMode();
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
        var sourceDefaultState = prevScreen.GetDefaultVisualState();
        var targetDefaultState = nextScreen.GetDefaultVisualState();
        var (sourceState, targetState) = InterpolateVisualState(
            sourceDefaultState,
            targetDefaultState,
            _progress
        );

        // 应用过渡状态, 控制前后界面的过渡效果
        prevScreen.ApplyVisualState(sourceState);
        nextScreen.ApplyVisualState(targetState);
    }
}
