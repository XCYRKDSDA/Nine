namespace Nine.Screens.Transitions;

/// <summary>
/// 过渡画面基类. 可以通过直接传入新画面实例的方式构造, 也可以通过传入新画面构造任务的方式构造
/// </summary>
public abstract class TransitionBase : ScreenBase
{
    protected TransitionBase(ScreenManager screenManager,
                             IScreen prevScreen, IScreen nextScreen,
                             object? context = null)
        : base(screenManager)
    {
        PrevScreen = prevScreen;

        _context = context;
        _nextScreenLoader = null;
        _nextScreen = nextScreen;
    }

    protected TransitionBase(ScreenManager screenManager,
                             IScreen prevScreen, Task<IScreen> nextScreenLoader,
                             object? context = null)
        : base(screenManager)
    {
        PrevScreen = prevScreen;

        _context = context;
        _nextScreenLoader = nextScreenLoader;
        _nextScreen = null;
    }

    public IScreen PrevScreen { get; }

    private object? _context;
    private Task<IScreen>? _nextScreenLoader;
    private IScreen? _nextScreen;

    /// <summary>
    /// 下一个画面.
    /// 如果新画面加载任务已完成, 则返回加载而得的任务;
    /// 如果手动设置了新画面, 则返回手动设置的值.
    /// 除此之外返回<c>null</c>
    /// </summary>
    public IScreen? NextScreen
    {
        get
        {
            if (_nextScreenLoader != null && _nextScreenLoader.IsCompleted)
            {
                _nextScreen = _nextScreenLoader.Result;
                _nextScreenLoader = null;
            }

            return _nextScreen;
        }
    }

    public override void OnActivated()
    {
        base.OnActivated();
        PrevScreen.OnStartTransitOut(_context);
        NextScreen?.OnStartTransitIn(_context);
    }
}
