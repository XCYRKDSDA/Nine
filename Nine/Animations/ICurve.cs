namespace Nine.Animations;

/// <summary>
/// 不含值的类型信息的曲线接口
/// </summary>
public interface ICurve;

/// <summary>
/// 具体指定了值的类型的泛型曲线接口
/// </summary>
/// <typeparam name="TValue">曲线输出的值的类型</typeparam>
public interface ICurve<TValue> : ICurve
{
    ICurveKeyCollection<TValue> Keys { get; }

    TValue Evaluate(float position);
}
