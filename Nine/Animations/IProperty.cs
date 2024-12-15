namespace Nine.Animations;

/// <summary>
/// 不含属性的类型信息的属性接口
/// </summary>
/// <typeparam name="ObjectT">属性所适用的对象的类型</typeparam>
public interface IProperty<ObjectT>
{ }

/// <summary>
/// 具体指定了适用对象和属性的值的类型的属性接口
/// </summary>
/// <typeparam name="ObjectT">属性所适用的对象的类型</typeparam>
/// <typeparam name="ValueT">对象的属性的值的类型</typeparam>
public interface IProperty<ObjectT, ValueT> : IProperty<ObjectT>
{
    void Set(ref ObjectT obj, in ValueT value);

    ValueT Get(in ObjectT obj);
}
