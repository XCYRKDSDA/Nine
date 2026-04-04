using Microsoft.Xna.Framework;

namespace Nine.Screens;

/// <summary>
/// 画面管理器. 负责画面调用和切换
/// </summary>
public class ScreenManager : DrawableGameComponent
{
    public ScreenManager(Game game)
        : base(game) { }

    private readonly Stack<IScreen> _screenStack = [];

    private Action? _screenAction = null;

    private IScreen? _activeScreen = null;

    public IScreen? PreviousScreen => _screenStack.Count > 0 ? _screenStack.Peek() : null;

    public IScreen? ActiveScreen => _activeScreen;

    public void PushScreen(IScreen nextScreen)
    {
        _screenAction = () =>
        {
            if (_activeScreen is not null)
            {
                _activeScreen.OnDeactivated();
                _screenStack.Push(_activeScreen);
            }
            _activeScreen = nextScreen;
            _activeScreen.OnActivated();
        };
    }

    public void PopScreen()
    {
        _screenAction = () =>
        {
            if (_activeScreen is null)
                throw new InvalidOperationException("No screen to pop!");
            _activeScreen.OnDeactivated();
            _activeScreen.Dispose();

            _activeScreen = _screenStack.Pop();
            _activeScreen.OnActivated();
        };
    }

    public void ReplaceScreen(IScreen nextScreen)
    {
        _screenAction = () =>
        {
            if (_activeScreen is not null)
            {
                _activeScreen.OnDeactivated();
                _activeScreen.Dispose();
            }

            _activeScreen = nextScreen;
            _activeScreen.OnActivated();
        };
    }

    public void ClearStack()
    {
        _screenAction = () =>
        {
            if (_activeScreen is null)
                return;

            _activeScreen.OnDeactivated();
            _activeScreen.Dispose();
            while (_screenStack.Count > 0)
                _screenStack.Pop().Dispose();
        };
    }

    public override void Update(GameTime gameTime)
    {
        if (_screenAction is not null)
        {
            _screenAction.Invoke();
            _screenAction = null;
        }
        _activeScreen?.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        _activeScreen?.Draw(gameTime);
    }
}
