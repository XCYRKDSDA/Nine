namespace Nine.Screens;

/// <summary>
/// 可接收导航参数的界面. 导航携带参数时调用一次
/// </summary>
public interface IQueryAttributable
{
    void ApplyQueryAttributes(IReadOnlyDictionary<string, object> query);
}
