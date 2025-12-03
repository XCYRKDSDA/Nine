namespace Nine.Screens;

/// <summary>
/// 过渡画面基类. 可以通过直接传入新画面实例的方式构造, 也可以通过传入新画面构造任务的方式构造
/// </summary>
public abstract class TransitionBase(
    ScreenManager screenManager, IScreen prevScreen, IScreen nextScreen, object? context = null)
    : ScreenBase(screenManager)
{
    public IScreen PrevScreen => prevScreen;

    public IScreen NextScreen => nextScreen;

    public override void OnActivated()
    {
        base.OnActivated();
        prevScreen.OnStartTransitOut(context);
        nextScreen.OnStartTransitIn(context);
    }

    public override void OnDeactivated()
    {
        base.OnDeactivated();
        prevScreen.OnFinishTransitOut(context);
        nextScreen.OnFinishTransitIn(context);
    }

    protected void UpdateTransition(float progress)
    {
        prevScreen.OnTransitOut(context, progress);
        nextScreen.OnTransitIn(context, progress);
    }
}
