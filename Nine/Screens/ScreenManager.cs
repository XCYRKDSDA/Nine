using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Nine.Screens;

internal enum NavigationDirection
{
    Push,
    Pop,
    Swap,
}

internal interface INavigation;

internal record NavigationWithoutTransition(
    NavigationDirection Direction,
    IScreen? PrevScreen,
    IScreen NextScreen
) : INavigation;

internal record NavigationWithTransition(
    NavigationDirection Direction,
    IScreen PrevScreen,
    ITransitionScreen TransitionScreen,
    IScreen NextScreen
) : INavigation;

internal record NavigationWithAsynchronousTransition(
    NavigationDirection Direction,
    IScreen PrevScreen,
    ITransitionScreen TransitionScreen,
    ITaskLike<IScreen> NextScreenTask
) : INavigation;

public sealed class ScreenManager(IScreenFactory screenFactory, Game game)
    : DrawableGameComponent(game)
{
    // 界面栈. 存储不活跃的那些界面
    private readonly Stack<IScreen> _screenStack = [];

    private INavigation? _navigationRequest = null;

    private object? _activeStatus = null;

    public IScreen? ActiveScreen =>
        _activeStatus switch
        {
            null => null,
            IScreen screen => screen,
            NavigationWithoutTransition nav1 => nav1.PrevScreen,
            NavigationWithTransition nav2 => nav2.TransitionScreen,
            NavigationWithAsynchronousTransition nav3 => nav3.TransitionScreen,
            _ => throw new NotImplementedException(),
        };

    public void Forward(
        Type targetScreenType,
        object? context = null,
        Type? transitionScreenType = null,
        object? transitionArguments = null,
        bool swap = false
    )
    {
        Debug.Assert(targetScreenType.IsAssignableTo(typeof(IScreen)));

        if (_navigationRequest is INavigation)
            throw new InvalidOperationException("当前正在发生导航, 禁止再次发生导航!");

        // 获取当前界面
        var currentScreen = _activeStatus as IScreen;

        // 创建下一个界面
        var targetScreen = screenFactory.CreateScreen(targetScreenType, context);

        if (transitionScreenType is null)
        {
            // 若无须过渡, 则直接切换到下一个界面
            _navigationRequest = new NavigationWithoutTransition(
                swap ? NavigationDirection.Swap : NavigationDirection.Push,
                currentScreen,
                targetScreen
            );
        }
        else
        {
            // 若指定了过渡, 则创建并切换到过渡界面
            Debug.Assert(transitionScreenType.IsAssignableTo(typeof(ITransitionScreen)));
            if (currentScreen is null)
                throw new InvalidOperationException("当前界面为空, 无法创建过渡!");
            var transitionScreen = screenFactory.CreateTransitionScreen(
                transitionScreenType,
                currentScreen,
                targetScreen,
                transitionArguments
            );
            _navigationRequest = new NavigationWithTransition(
                swap ? NavigationDirection.Swap : NavigationDirection.Push,
                currentScreen,
                transitionScreen,
                targetScreen
            );
        }
    }

    public void Forward2(
        Type targetScreenType,
        Task<object?> contextTask,
        Type transitionScreenType,
        object? transitionArguments = null,
        bool swap = false
    )
    {
        Debug.Assert(targetScreenType.IsAssignableTo(typeof(IScreen)));

        if (_navigationRequest is INavigation)
            throw new InvalidOperationException("当前正在发生导航, 禁止再次发生导航!");

        // 获取当前界面
        var currentScreen = _activeStatus as IScreen;

        // 创建下一个界面
        var targetScreenTask = screenFactory.CreateScreen2(targetScreenType, contextTask);

        // 创建并切换到过渡界面
        Debug.Assert(transitionScreenType.IsAssignableTo(typeof(ITransitionScreen)));
        if (currentScreen is null)
            throw new InvalidOperationException("当前界面为空, 无法创建过渡!");
        var transitionScreen = screenFactory.CreateTransitionScreen2(
            transitionScreenType,
            currentScreen,
            targetScreenTask,
            transitionArguments
        );
        _navigationRequest = new NavigationWithAsynchronousTransition(
            swap ? NavigationDirection.Swap : NavigationDirection.Push,
            currentScreen,
            transitionScreen,
            targetScreenTask
        );
    }

    public void Backward(Type? transitionScreenType = null, object? transitionArguments = null)
    {
        // 获取当前界面
        var currentScreen = (IScreen)_activeStatus!;

        if (_navigationRequest is INavigation)
            throw new InvalidOperationException("当前正在发生导航, 禁止再次发生导航!");

        // 获取上一个界面
        if (_screenStack.Count == 0)
            throw new InvalidOperationException("当前界面栈中没有暂存的界面, 无法返回!");
        var targetScreen = _screenStack.Peek();

        if (transitionScreenType is null)
        {
            // 若无须过渡, 则直接切换到下一个界面
            _navigationRequest = new NavigationWithoutTransition(
                NavigationDirection.Pop,
                currentScreen,
                targetScreen
            );
        }
        else
        {
            // 若指定了过渡, 则创建并切换到过渡界面
            Debug.Assert(transitionScreenType.IsAssignableTo(typeof(ITransitionScreen)));
            var transitionScreen = screenFactory.CreateTransitionScreen(
                transitionScreenType,
                currentScreen,
                targetScreen,
                transitionArguments
            );
            _navigationRequest = new NavigationWithTransition(
                NavigationDirection.Pop,
                currentScreen,
                transitionScreen,
                targetScreen
            );
        }
    }

    private void StartNavigation()
    {
        Debug.Assert(_navigationRequest is not null);

        IScreen? prevScreen;
        ITransitionScreen? transitionScreen = null;

        if (_navigationRequest is NavigationWithoutTransition navigation1)
            (_, prevScreen, _) = navigation1;
        else if (_navigationRequest is NavigationWithTransition navigation2)
            (_, prevScreen, transitionScreen, _) = navigation2;
        else if (_navigationRequest is NavigationWithAsynchronousTransition navigation3)
            (_, prevScreen, transitionScreen, _) = navigation3;
        else
            throw new NotImplementedException();

        prevScreen?.OnDeactivated();
        transitionScreen?.OnActivated();

        _activeStatus = _navigationRequest;
        _navigationRequest = null;
    }

    private bool CheckNavigationDone()
    {
        return _activeStatus switch
        {
            null or IScreen => false,
            NavigationWithoutTransition nav1 => true,
            NavigationWithTransition nav2 => nav2.TransitionScreen.TransitionDone,
            NavigationWithAsynchronousTransition nav3 => nav3.TransitionScreen.TransitionDone
                && nav3.NextScreenTask.IsCompleted,
            _ => throw new NotImplementedException(),
        };
    }

    private void FinalizeNavigation()
    {
        Debug.Assert(_activeStatus is INavigation);

        NavigationDirection dir;
        IScreen? prevScreen;
        ITransitionScreen? transitionScreen = null;
        IScreen nextScreen;

        if (_activeStatus is NavigationWithoutTransition navigation1)
        {
            // 当导航无须过渡时, 总是执行导航
            (dir, prevScreen, nextScreen) = navigation1;
        }
        else if (_activeStatus is NavigationWithTransition navigation2)
        {
            (dir, prevScreen, transitionScreen, nextScreen) = navigation2;
            Debug.Assert(transitionScreen.TransitionDone);
        }
        else if (_activeStatus is NavigationWithAsynchronousTransition navigation3)
        {
            ITaskLike<IScreen> nextScreenTask;
            (dir, prevScreen, transitionScreen, nextScreenTask) = navigation3;
            Debug.Assert(transitionScreen.TransitionDone && nextScreenTask.IsCompleted);
            nextScreen = nextScreenTask.Result;
        }
        else
            throw new NotImplementedException();

        transitionScreen?.OnDeactivated();
        transitionScreen?.Dispose();
        if (prevScreen is not null)
        {
            if (dir is NavigationDirection.Push)
                _screenStack.Push(prevScreen);
            else if (dir is NavigationDirection.Swap)
                prevScreen.Dispose();
            else if (dir is NavigationDirection.Pop)
            {
                prevScreen.Dispose();
                _screenStack.Pop();
            }
            else
                throw new NotImplementedException();
        }
        _activeStatus = nextScreen;
        nextScreen.OnActivated();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_navigationRequest is not null)
            StartNavigation();
        if (CheckNavigationDone())
            FinalizeNavigation();

        ActiveScreen?.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        ActiveScreen?.Draw(gameTime);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            // TODO: 释放栈中所有界面
        }
    }
}
