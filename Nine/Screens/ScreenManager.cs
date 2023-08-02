using Microsoft.Xna.Framework;

namespace Nine.Screens;

/// <summary>
/// 画面管理器. 负责画面调用和切换
/// </summary>
public class ScreenManager : DrawableGameComponent
{
    public ScreenManager(Game game) : base(game)
    { }

    private IScreen? _activeScreen;
    private IScreen? _nextActiveScreen;

    public IScreen? ActiveScreen
    {
        get => _activeScreen;
        set => _nextActiveScreen = value;
    }

    public override void Update(GameTime gameTime)
    {
        // 如果有新的画面, 则进行切换
        if (_nextActiveScreen != null)
        {
            _activeScreen?.OnDeactivated();
            _nextActiveScreen.OnActivated();
            _activeScreen = _nextActiveScreen;
            _nextActiveScreen = null;
        }

        _activeScreen?.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        _activeScreen?.Draw(gameTime);
    }
}
