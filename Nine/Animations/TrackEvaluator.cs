namespace Nine.Animations;

public static class TrackEvaluator<ObjectT>
{
    public static void EvaluateAndSet<ValueT>(ref ObjectT obj, IProperty<ObjectT, ValueT> property, ICurve<ValueT>? curve, float t)
    {
        var value = curve == null ? property.Get(in obj) : curve.Evaluate(t);
        property.Set(ref obj, value);
    }

    public static void TweenAndSet<ValueT>(ref ObjectT obj, IProperty<ObjectT, ValueT> property,
                                           ICurve<ValueT>? curve1, float t1, ICurve<ValueT>? curve2, float t2,
                                           ITweener<ValueT> tweener, float k)
    {
        var value1 = curve1 == null ? property.Get(in obj) : curve1.Evaluate(t1);
        var value2 = curve2 == null ? property.Get(in obj) : curve2.Evaluate(t2);
        property.Set(ref obj, tweener.Tween(in value1, in value2, k));
    }

    #region Reflection Cache

    private delegate void EvaluateAndSetMethod(ref ObjectT obj, IProperty<ObjectT> property, ICurve? curve, float t);
    private delegate void TweenAndSetMethod(ref ObjectT obj, IProperty<ObjectT> property, ICurve? curve1, float t1, ICurve? curve2, float t2, ITweener tweener, float k);

    private static readonly Dictionary<Type, (EvaluateAndSetMethod, TweenAndSetMethod)> _cachedMethods = new();

    private static (EvaluateAndSetMethod, TweenAndSetMethod) GetEvaluationMethodsFromCache(Type valueType)
    {
        // 不重复记录
        if (_cachedMethods.TryGetValue(valueType, out var methods))
            return methods;

        // 获得方法并生成委托
        var evaluateAndSetHandlerInfo = typeof(TrackEvaluator<ObjectT>).GetMethod("EvaluateAndSet")!;
        var evaluateAndSetHandler = (EvaluateAndSetMethod)Delegate.CreateDelegate(typeof(EvaluateAndSetMethod), evaluateAndSetHandlerInfo);
        var evaluateAndTweenAndSetHandlerInfo = typeof(TrackEvaluator<ObjectT>).GetMethod("EvaluateAndTweenAndSet")!;
        var evaluateAndTweenAndSetHandler = (TweenAndSetMethod)Delegate.CreateDelegate(typeof(EvaluateAndSetMethod), evaluateAndTweenAndSetHandlerInfo);

        // 缓存委托
        _cachedMethods.Add(valueType, (evaluateAndSetHandler, evaluateAndTweenAndSetHandler));

        return (evaluateAndSetHandler, evaluateAndTweenAndSetHandler);
    }

    public static void CacheEvaluationMethods(Type valueType) => _ = GetEvaluationMethodsFromCache(valueType);

    #endregion

    public static void EvaluateAndSet(ref ObjectT obj, IProperty<ObjectT> property, Type valueType, ICurve? curve, float t)
    {
        var (method, _) = GetEvaluationMethodsFromCache(valueType);
        method.Invoke(ref obj, property, curve, t);
    }

    public static void TweenAndSet(ref ObjectT obj, IProperty<ObjectT> property, Type valueType,
                                   ICurve? curve1, float t1, ICurve? curve2, float t2,
                                   ITweener tweener, float k)
    {
        var (_, method) = GetEvaluationMethodsFromCache(valueType);
        method.Invoke(ref obj, property, curve1, t1, curve2, t2, tweener, k);
    }

    public static void EvaluateAndSet(ref ObjectT obj, TrackCollection<ObjectT> tracks, float t)
    {
        foreach (var (property, valueType, curve) in tracks)
            EvaluateAndSet(ref obj, property, valueType, curve, t);
    }

    public static void TweenAndSet(ref ObjectT obj,
                                   TrackCollection<ObjectT> tracks1, float t1, TrackCollection<ObjectT> tracks2, float t2,
                                   ITweener tweener, float k)
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
