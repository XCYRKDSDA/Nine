using Microsoft.Xna.Framework;

namespace Nine.Screens;

/// <summary>
/// 过渡界面基类
/// </summary>
/// <typeparam name="TState">过渡状态类型. 常用于控制前后界面过渡动画</typeparam>
public abstract class StatefulTransitionScreenBase<TState>(
    ScreenManager screenManager,
    IConfigurableScreen<TState> prevScreen,
    IConfigurableScreen<TState> nextScreen
) : ScreenBase(screenManager)
    where TState : struct
{
    public IConfigurableScreen<TState> PrevScreen => prevScreen;

    public IConfigurableScreen<TState> NextScreen => nextScreen;

    private float _progress = 0;

    public float Progress => _progress;

    protected abstract float UpdateProgress(GameTime gameTime);

    protected abstract TState TransitionState(float progress);

    public override void Update(GameTime gameTime)
    {
        // 前后界面各自照常更新
        prevScreen.Update(gameTime);
        nextScreen.Update(gameTime);

        // 计算过渡进度和过渡状态
        _progress = UpdateProgress(gameTime);
        if (_progress >= 1)
        {
            // 完成过渡, 切换活跃界面
            ScreenManager.ActiveScreen = NextScreen;
            _progress = 1;
        }
        var transitionState = TransitionState(_progress);

        // 应用过渡状态, 控制前后界面的过渡效果
        prevScreen.ApplyState(in transitionState);
        nextScreen.ApplyState(in transitionState);
    }
}
