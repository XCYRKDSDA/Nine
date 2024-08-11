namespace Nine.Animations;

/// <summary>
/// 不含值的类型信息的参数化曲线接口
/// </summary>
public interface IParametricCurve : IParametric<ICurve>, ICurve
{ }

/// <summary>
/// 具体指定了值的类型的泛型曲线接口
/// </summary>
/// <typeparam name="TValue">曲线输出的值的类型</typeparam>
public interface IParametricCurve<TValue> : IParametricCurve, IParametric<ICurve<TValue>>, ICurve<TValue>
{
    ICurve IParametric<ICurve>.Bake(IDictionary<string, object?>? parameters)
        => (this as IParametric<ICurve<TValue>>).Bake(parameters);
}
