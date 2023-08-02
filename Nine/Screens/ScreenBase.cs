using Microsoft.Xna.Framework;

namespace Nine.Screens;

public abstract class ScreenBase : IScreen
{
    protected ScreenBase(Game game, ScreenManager screenManager)
    {
        Game = game;
        ScreenManager = screenManager;
    }

    public Game Game { get; private set; }

    public ScreenManager ScreenManager { get; private set; }

    public virtual void OnActivated() { }
    public virtual void OnDeactivated() { }

    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime);
}
