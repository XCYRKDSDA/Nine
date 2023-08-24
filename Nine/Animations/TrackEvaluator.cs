using System.Reflection;

namespace Nine.Animations;

using static GenericMathHelper;

public static class TrackEvaluator
{
    public static void EvaluateAndSet<ObjectT, ValueT>(ref ObjectT obj, IProperty<ObjectT, ValueT> property, ICurve<ValueT>? curve, float t)
    {
        var value = curve == null ? property.Get(in obj) : curve.Evaluate(t);
        property.Set(ref obj, value);
    }

    public static void TweenAndSet<ObjectT, ValueT>(ref ObjectT obj, IProperty<ObjectT, ValueT> property,
                                                    ICurve<ValueT>? curve1, float t1, ICurve<ValueT>? curve2, float t2,
                                                    ICurve<float>? tweener, float k)
    {
        // 获取两条曲线各自的输出
        var value1 = curve1 == null ? property.Get(in obj) : curve1.Evaluate(t1);
        var value2 = curve2 == null ? property.Get(in obj) : curve2.Evaluate(t2);

        // 如果没有给定插值器, 则按照线性插值. 否则使用给定的插值器进行映射
        if (tweener != null)
            k = tweener.Evaluate(k);

        // 混合两条曲线的输出, 并赋值
        var result = Add(Mul(in value1, 1 - k), Mul(in value2, k)); ;
        property.Set(ref obj, in result);
    }

    #region Evaluation Methods Cache

    private delegate void EvaluateAndSetMethod<ObjectT>(ref ObjectT obj, IProperty<ObjectT> property, ICurve? curve, float t);
    private delegate void TweenAndSetMethod<ObjectT>(ref ObjectT obj, IProperty<ObjectT> property,
                                                     ICurve? curve1, float t1, ICurve? curve2, float t2, ICurve<float>? tweener, float k);

    private static readonly Dictionary<(Type, Type), (Delegate, Delegate)> _cachedMethods = new();

    private static void EvaluateAndSet_Wrapper<ObjectT, ValueT>(ref ObjectT obj, IProperty<ObjectT> property, ICurve? curve, float t)
        => EvaluateAndSet(ref obj, (IProperty<ObjectT, ValueT>)property, (ICurve<ValueT>?)curve, t);

    private static void TweenAndSet_Wrapper<ObjectT, ValueT>(ref ObjectT obj, IProperty<ObjectT> property,
                                                             ICurve? curve1, float t1, ICurve? curve2, float t2, ICurve<float>? tweener, float k)
        => TweenAndSet(ref obj, (IProperty<ObjectT, ValueT>)property, (ICurve<ValueT>?)curve1, t1, (ICurve<ValueT>?)curve2, t2, tweener, k);

    private static readonly MethodInfo EvaluateAndSet_Wrapper_Info = typeof(TrackEvaluator).GetMethod("EvaluateAndSet_Wrapper", BindingFlags.NonPublic | BindingFlags.Static)!;
    private static readonly MethodInfo TweenAndSet_Wrapper_Info = typeof(TrackEvaluator).GetMethod("TweenAndSet_Wrapper", BindingFlags.NonPublic | BindingFlags.Static)!;

    private static (Delegate, Delegate) GetEvaluationMethodsFromCache(Type objectType, Type valueType)
    {
        // 不重复记录
        if (_cachedMethods.TryGetValue((objectType, valueType), out var methods))
            return methods;

        // 获得方法并生成委托
        var evaluateAndSetHandlerInfo = EvaluateAndSet_Wrapper_Info.MakeGenericMethod(objectType, valueType)!;
        var evaluateAndSetHandler = Delegate.CreateDelegate(typeof(EvaluateAndSetMethod<>).MakeGenericType(objectType), evaluateAndSetHandlerInfo);
        var tweenAndSetHandlerInfo = TweenAndSet_Wrapper_Info.MakeGenericMethod(objectType, valueType)!;
        var tweenAndSetHandler = Delegate.CreateDelegate(typeof(TweenAndSetMethod<>).MakeGenericType(objectType), tweenAndSetHandlerInfo);

        // 缓存委托
        _cachedMethods.Add((objectType, valueType), (evaluateAndSetHandler, tweenAndSetHandler));

        return (evaluateAndSetHandler, tweenAndSetHandler);
    }

    public static void CacheEvaluationMethods(Type objectType, Type valueType) => _ = GetEvaluationMethodsFromCache(objectType, valueType);

    #endregion

    public static void EvaluateAndSet<ObjectT>(ref ObjectT obj, IProperty<ObjectT> property, Type valueType, ICurve? curve, float t)
    {
        var (method, _) = GetEvaluationMethodsFromCache(typeof(ObjectT), valueType);
        ((EvaluateAndSetMethod<ObjectT>)method).Invoke(ref obj, property, curve, t);
    }

    public static void TweenAndSet<ObjectT>(ref ObjectT obj, IProperty<ObjectT> property, Type valueType,
                                            ICurve? curve1, float t1, ICurve? curve2, float t2, ICurve<float>? tweener, float k)
    {
        var (_, method) = GetEvaluationMethodsFromCache(typeof(ObjectT), valueType);
        ((TweenAndSetMethod<ObjectT>)method).Invoke(ref obj, property, curve1, t1, curve2, t2, tweener, k);
    }

    public static void EvaluateAndSet<ObjectT>(ref ObjectT obj, TrackCollection<ObjectT> tracks, float t)
    {
        foreach (var ((property, valueType), curve) in tracks)
            EvaluateAndSet(ref obj, property, valueType, curve, t);
    }

    public static void TweenAndSet<ObjectT>(ref ObjectT obj,
                                            TrackCollection<ObjectT> tracks1, float t1, TrackCollection<ObjectT> tracks2, float t2,
                                            ICurve<float>? tweener, float k)
    {
        // 遍历两个轨道集合关注的属性的交集
        var unionedKeys = Enumerable.Union(tracks1.Keys, tracks2.Keys);
        foreach (var (property, valueType) in unionedKeys)
        {
            var curve1 = tracks1!.GetValueOrDefault((property, valueType), null);
            var curve2 = tracks2!.GetValueOrDefault((property, valueType), null);

            TweenAndSet(ref obj, property, valueType, curve1, t1, curve2, t2, tweener, k);
        }
    }
}
