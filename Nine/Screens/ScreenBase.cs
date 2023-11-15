using Microsoft.Xna.Framework;

namespace Nine.Screens;

public abstract class ScreenBase : IScreen
{
    protected ScreenBase(ScreenManager screenManager)
    {
        ScreenManager = screenManager;
    }

    public ScreenManager ScreenManager { get; private set; }

    public virtual void OnActivated() { }
    public virtual void OnDeactivated() { }

    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime);
}
