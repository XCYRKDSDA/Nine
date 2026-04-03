using System.Diagnostics;

namespace Nine.Screens;

public class NavigationService(ScreenManager screenManager, IScreenFactory screenFactory)
{
    public void Navigate(
        Type screenType,
        object? context = null,
        Type? transitionScreenType = null,
        object? transitionArguments = null
    )
    {
        Debug.Assert(screenType.GetInterfaces().Contains(typeof(IScreen)));

        // 获取当前界面
        var currentScreen = screenManager.ActiveScreen!;

        // 如果当前界面是过渡界面, 则取其下一个界面为当前界面
        if (currentScreen is ITransitionScreen currentTransitionScreen)
            currentScreen = currentTransitionScreen.NextScreen!;

        var currentScreenType = currentScreen.GetType();

        // 创建下一个界面
        var targetScreen = screenFactory.CreateScreen(screenType, context);

        if (transitionScreenType is null)
        {
            // 若无须过渡, 则直接切换到下一个界面
            screenManager.ActiveScreen = targetScreen;
        }
        else
        {
            // 若指定了过渡, 则创建并切换到过渡界面
            Debug.Assert(transitionScreenType.GetInterfaces().Contains(typeof(ITransitionScreen)));
            var transitionScreen = screenFactory.CreateTransitionScreen(
                transitionScreenType,
                currentScreen,
                targetScreen,
                transitionArguments
            );
            transitionScreen.TransitionDone += OnTransitionDone;
            screenManager.ActiveScreen = transitionScreen;
        }
    }

    public void Navigate2(
        Type screenType,
        Task<object?> contextTask,
        Type transitionScreenType,
        object? transitionArguments = null
    )
    {
        Debug.Assert(screenType.IsAssignableTo(typeof(IScreen)));

        // 获取当前界面
        var currentScreen = screenManager.ActiveScreen!;

        // 如果当前界面是过渡界面, 则取其下一个界面为当前界面
        if (currentScreen is ITransitionScreen currentTransitionScreen)
            currentScreen = currentTransitionScreen.NextScreen!;

        var currentScreenType = currentScreen.GetType();

        // 创建下一个界面的任务
        var targetScreenTask = screenFactory.CreateScreen2(screenType, contextTask);

        // 创建并切换到过渡界面
        Debug.Assert(transitionScreenType.IsAssignableTo(typeof(ITransitionScreen)));
        var transitionScreen = screenFactory.CreateTransitionScreen2(
            transitionScreenType,
            currentScreen,
            targetScreenTask,
            transitionArguments
        );
        transitionScreen.TransitionDone += OnTransitionDone;
        screenManager.ActiveScreen = transitionScreen;
    }

    private void OnTransitionDone(object? sender, EventArgs e)
    {
        Debug.Assert(sender is ITransitionScreen);
        Debug.Assert(ReferenceEquals(sender, screenManager.ActiveScreen));
        var transitionScreen = (ITransitionScreen)sender;
        screenManager.ActiveScreen = transitionScreen.NextScreen;
        transitionScreen.TransitionDone -= OnTransitionDone;
    }
}
