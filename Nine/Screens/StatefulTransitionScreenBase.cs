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

    protected abstract (TSourceState SourceState, TTargetState TargetState) UpdateVisualState(
        TSourceState? sourceDefaultState,
        TTargetState? targetDefaultState
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

        // 插值得到当前过渡状态
        var sourceDefaultState = prevScreen.GetDefaultVisualState();
        var targetDefaultState = nextScreen.GetDefaultVisualState();
        var (sourceState, targetState) = UpdateVisualState(sourceDefaultState, targetDefaultState);

        // 应用过渡状态, 控制前后界面的过渡效果
        prevScreen.ApplyVisualState(sourceState);
        nextScreen.ApplyVisualState(targetState);
    }
}
