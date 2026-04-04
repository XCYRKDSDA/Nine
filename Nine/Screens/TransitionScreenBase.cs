using Microsoft.Xna.Framework;

namespace Nine.Screens;

/// <summary>
/// 过渡界面基类
/// </summary>
public abstract class TransitionScreenBase(IScreen prevScreen, IScreen nextScreen)
    : ITransitionScreen
{
    public IScreen PrevScreen => prevScreen;

    public IScreen NextScreen => nextScreen;

    public event EventHandler? TransitionDone;

    protected void OnTransitionDone()
    {
        TransitionDone?.Invoke(this, EventArgs.Empty);
    }

    public virtual void OnActivated() { }

    public virtual void OnDeactivated() { }

    public virtual void Update(GameTime gameTime)
    {
        // 前后界面各自照常更新
        prevScreen.Update(gameTime);
        nextScreen.Update(gameTime);
    }

    public abstract void Draw(GameTime gameTime);

    public virtual void Dispose() { }
}
