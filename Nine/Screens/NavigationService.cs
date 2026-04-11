using System.Diagnostics;

namespace Nine.Screens;

public class NavigationService(ScreenManager screenManager, IScreenFactory screenFactory)
{
    public void Forward(
        Type screenType,
        object? context = null,
        Type? transitionScreenType = null,
        object? transitionArguments = null,
        bool swap = false
    )
    {
        Debug.Assert(screenType.GetInterfaces().Contains(typeof(IScreen)));

        // 获取当前界面
        var currentScreen = screenManager.ActiveScreen!;

        // 选择默认行为
        Action<IScreen> action = swap ? s => screenManager.SwapScreen(s) : screenManager.PushScreen;

        if (currentScreen is ITransitionScreen currentTransitionScreen)
        {
            // 如果当前界面是过渡界面, 则取其下一个界面为当前界面, 且动作强制为 swap
            currentScreen = currentTransitionScreen.NextScreen!;
            action = s => screenManager.SwapScreen(s);
        }

        var currentScreenType = currentScreen.GetType();

        // 创建下一个界面
        var targetScreen = screenFactory.CreateScreen(screenType, context);

        if (transitionScreenType is null)
        {
            // 若无须过渡, 则直接切换到下一个界面
            action.Invoke(targetScreen);
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
            transitionScreen.TransitionDone += OnForwardTransitionDone;
            action.Invoke(transitionScreen);
        }
    }

    public void Forward2(
        Type screenType,
        Task<object?> contextTask,
        Type transitionScreenType,
        object? transitionArguments = null,
        bool swap = false
    )
    {
        Debug.Assert(screenType.IsAssignableTo(typeof(IScreen)));

        // 获取当前界面
        var currentScreen = screenManager.ActiveScreen!;

        // 选择默认行为
        Action<IScreen> action = swap ? s => screenManager.SwapScreen(s) : screenManager.PushScreen;

        if (currentScreen is ITransitionScreen currentTransitionScreen)
        {
            // 如果当前界面是过渡界面, 则取其下一个界面为当前界面, 且动作改为 swap
            currentScreen = currentTransitionScreen.NextScreen!;
            action = s => screenManager.SwapScreen(s);
        }

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
        transitionScreen.TransitionDone += OnForwardTransitionDone;
        action.Invoke(transitionScreen);
    }

    public void Backward(Type? transitionScreenType = null, object? transitionArguments = null)
    {
        // 获取当前界面
        var currentScreen = screenManager.ActiveScreen!;

        // 如果当前界面是过渡界面, 则取其下一个界面为当前界面
        if (currentScreen is ITransitionScreen currentTransitionScreen)
            currentScreen = currentTransitionScreen.NextScreen!;

        var currentScreenType = currentScreen.GetType();

        // 创建上一个界面
        var targetScreen = screenManager.PreviousScreen;

        if (transitionScreenType is null)
        {
            // 若无须过渡, 则直接切换到下一个界面
            if (targetScreen is not null)
                screenManager.PopScreen();
            // 若目标界面为空, 即没有上一个界面了, 则没什么要做的
        }
        else
        {
            // 若指定了过渡, 则创建并切换到过渡界面
            Debug.Assert(transitionScreenType.GetInterfaces().Contains(typeof(ITransitionScreen)));
            Debug.Assert(targetScreen is not null); // 必须存在上一个界面
            var transitionScreen = screenFactory.CreateTransitionScreen(
                transitionScreenType,
                currentScreen,
                targetScreen,
                transitionArguments
            );
            transitionScreen.TransitionDone += OnBackwardTransitionDone;
            screenManager.SwapScreen(transitionScreen, dispose: false); // 不立刻释放当前界面
        }
    }

    private void OnForwardTransitionDone(object? sender, EventArgs e)
    {
        Debug.Assert(sender is ITransitionScreen);
        Debug.Assert(ReferenceEquals(sender, screenManager.ActiveScreen));
        var transitionScreen = (ITransitionScreen)sender;
        transitionScreen.TransitionDone -= OnForwardTransitionDone;
        screenManager.SwapScreen(transitionScreen.NextScreen!); // 过渡界面总是次抛
    }

    private void OnBackwardTransitionDone(object? sender, EventArgs e)
    {
        Debug.Assert(sender is ITransitionScreen);
        Debug.Assert(ReferenceEquals(sender, screenManager.ActiveScreen));
        var transitionScreen = (ITransitionScreen)sender;
        Debug.Assert(ReferenceEquals(transitionScreen.NextScreen, screenManager.PreviousScreen));
        transitionScreen.TransitionDone -= OnBackwardTransitionDone;
        transitionScreen.PrevScreen.Dispose(); // 释放之前的界面
        screenManager.PopScreen(); // 直接回到栈中上一个界面
    }
}
