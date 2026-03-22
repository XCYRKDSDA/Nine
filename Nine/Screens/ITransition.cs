namespace Nine.Screens;

/// <summary>
/// 描述过渡过程的类型.
/// 其主要作用是将上一个界面声明的"源状态"插值到下一个界面的"目标状态"
/// </summary>
/// <typeparam name="TState">过渡所用状态类型</typeparam>
public interface ITransition<TState>
{
    /// <summary>
    /// 根据过渡进度, 在源界面状态和目标界面状态之间插值
    /// </summary>
    /// <param name="source">源界面状态</param>
    /// <param name="target">目标界面状态</param>
    /// <param name="progress">过渡进度</param>
    /// <returns></returns>
    TState InterpolateTransitionState(TState source, TState target, float progress);
}

/// <summary>
/// 提供"源界面状态"的接口
/// </summary>
/// <typeparam name="TState">该界面支持作为过渡源的过渡状态类型</typeparam>
public interface ITransitionSource<TState>
{
    TState GetSourceTransitionState();

    void OnStartTransition() { }

    void OnFinishTransition() { }
}

/// <summary>
/// 提供"目标界面状态"的接口
/// </summary>
/// <typeparam name="TState">该界面支持作为过渡目标的过渡状态类型</typeparam>
public interface ITransitionTarget<TState>
{
    TState GetTargetTransitionState();

    void OnStartTransition() { }

    void OnFinishTransition() { }
}
