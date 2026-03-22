namespace Nine.Screens;

/// <summary>
/// 提供"源界面状态"的接口
/// </summary>
/// <typeparam name="TState">该界面支持作为过渡源的过渡状态类型</typeparam>
public interface ITransitionSourceConstrained<TState>
{
    TState? GetTransitionSourceConstraint() => default;
}

/// <summary>
/// 提供"目标界面状态"的接口
/// </summary>
/// <typeparam name="TState">该界面支持作为过渡目标的过渡状态类型</typeparam>
public interface ITransitionTargetConstrained<TState>
{
    TState? GetTransitionTargetConstraint() => default;
}
