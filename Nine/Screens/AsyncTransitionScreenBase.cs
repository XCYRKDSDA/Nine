using Microsoft.Xna.Framework;

namespace Nine.Screens;

/// <summary>
/// 异步过渡界面基类
/// </summary>
public abstract class AsyncTransitionScreenBase(
    IScreen prevScreen,
    ITaskLike<IScreen> nextScreenTask
) : ITransitionScreen
{
    public IScreen PrevScreen => prevScreen;

    public IScreen? NextScreen =>
        nextScreenTask.IsCompletedSuccessfully ? nextScreenTask.Result : null;

    public bool TransitionDone { get; private set; }

    protected void OnTransitionDone()
    {
        TransitionDone = true;
    }

    public virtual void OnActivated() { }

    public virtual void OnDeactivated() { }

    public virtual void Update(GameTime gameTime)
    {
        // 前后界面各自照常更新
        prevScreen.Update(gameTime);
        NextScreen?.Update(gameTime);
    }

    public abstract void Draw(GameTime gameTime);

    public virtual void Dispose() { }
}
