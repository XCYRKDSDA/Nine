namespace Nine.Animations;

/// <summary>
/// 不含值的类型信息的补间接口
/// </summary>
public interface ITweener { }

/// <summary>
/// 指定了适用的值的类型的补间接口
/// </summary>
/// <typeparam name="ValueT">可使用该接口补间的值的类型</typeparam>
public interface ITweener<ValueT>
{
    ValueT Tween(in ValueT value1, in ValueT value2, float k);
}
