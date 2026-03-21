using Microsoft.Xna.Framework;

namespace Nine.Screens;

/// <summary>
/// 过渡界面基类
/// </summary>
public abstract class TransitionScreenBase(
    ScreenManager screenManager,
    IScreen prevScreen,
    IScreen nextScreen
) : ScreenBase(screenManager)
{
    public IScreen PrevScreen => prevScreen;

    public IScreen NextScreen => nextScreen;

    public override void Update(GameTime gameTime)
    {
        // 前后界面各自照常更新
        prevScreen.Update(gameTime);
        nextScreen.Update(gameTime);
    }
}
