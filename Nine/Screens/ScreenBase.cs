using Microsoft.Xna.Framework;

namespace Nine.Screens;

public abstract class ScreenBase : IScreen
{
    protected ScreenBase(NavigationService navigationService)
    {
        NavigationService = navigationService;
    }

    public NavigationService NavigationService { get; private set; }

    public virtual void OnActivated() { }

    public virtual void OnDeactivated() { }

    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime);
}
