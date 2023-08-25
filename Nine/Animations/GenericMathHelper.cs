using System.Linq.Expressions;
using System.Reflection;

namespace Nine.Animations;

using CacheTuple = ValueTuple<Delegate, Delegate, Delegate, Delegate, object>;

public static class GenericMathHelper
{
    private static readonly Dictionary<Type, CacheTuple> _cachedOperators = new();

    /// <summary>
    /// 使用表达式树生成加法
    /// </summary>
    /// <param name="valueType"></param>
    /// <returns></returns>
    private static Delegate CompileAdditionOperator(Type valueType)
    {
        var expr_a = Expression.Parameter(valueType, "a");
        var expr_b = Expression.Parameter(valueType, "b");
        var expr_addition = Expression.Add(expr_a, expr_b);
        return Expression.Lambda(expr_addition, expr_a, expr_b).Compile();
    }

    /// <summary>
    /// 使用表达式树生成减法
    /// </summary>
    /// <param name="valueType"></param>
    /// <returns></returns>
    private static Delegate CompileSubtractionOperator(Type valueType)
    {
        var expr_a = Expression.Parameter(valueType, "a");
        var expr_b = Expression.Parameter(valueType, "b");
        var expr_addition = Expression.Subtract(expr_a, expr_b);
        return Expression.Lambda(expr_addition, expr_a, expr_b).Compile();
    }

    /// <summary>
    /// 使用表达式树生成乘法
    /// </summary>
    /// <param name="valueType"></param>
    /// <returns></returns>
    private static Delegate CompileMultiplicationOperator(Type valueType)
    {
        var expr_a = Expression.Parameter(valueType, "a");
        var expr_k = Expression.Parameter(typeof(float), "k");
        var expr_multiply = Expression.Multiply(expr_a, expr_k);
        return Expression.Lambda(expr_multiply, expr_a, expr_k).Compile();
    }

    /// <summary>
    /// 使用表达式树生成除法
    /// </summary>
    /// <param name="valueType"></param>
    /// <returns></returns>
    private static Delegate CompileDivisionOperator(Type valueType)
    {
        var expr_a = Expression.Parameter(valueType, "a");
        var expr_k = Expression.Parameter(typeof(float), "k");
        var expr_multiply = Expression.Divide(expr_a, expr_k);
        return Expression.Lambda(expr_multiply, expr_a, expr_k).Compile();
    }

    private static object CompileZero(Type valueType, Delegate subtraction)
    {
        if (valueType.IsPrimitive)
            return Convert.ChangeType(0, valueType)!;

        if (valueType.GetField("Zero", BindingFlags.Static) is FieldInfo zeroField)
            return zeroField.GetValue(null)!;

        if (valueType.GetProperty("Zero", BindingFlags.Static) is PropertyInfo zeroProperty)
            return zeroProperty.GetValue(null)!;

        var val = Activator.CreateInstance(valueType)!;
        return subtraction.DynamicInvoke(val, val)!;
    }

    internal static CacheTuple GetOperatorsFromCache(Type valueType)
    {
        // 不重复记录
        if (_cachedOperators.TryGetValue(valueType, out var operators))
            return operators;

        // 生成委托
        var additionOperator = CompileAdditionOperator(valueType);
        var subtractionOperator = CompileSubtractionOperator(valueType);
        var multiplicationOperator = CompileMultiplicationOperator(valueType);
        var divisionOperator = CompileDivisionOperator(valueType);
        var zero = CompileZero(valueType, subtractionOperator);

        // 缓存委托
        operators = new(additionOperator, subtractionOperator, multiplicationOperator, divisionOperator, zero);
        _cachedOperators[valueType] = operators;

        return operators;
    }

    internal static (Func<ValueT, ValueT, ValueT>, Func<ValueT, ValueT, ValueT>, Func<ValueT, float, ValueT>, Func<ValueT, float, ValueT>, ValueT)
        GetOperatorsFromCache<ValueT>()
    {
        var (add, sub, mul, div, zero) = GetOperatorsFromCache(typeof(ValueT));
        return ((Func<ValueT, ValueT, ValueT>)add, (Func<ValueT, ValueT, ValueT>)sub,
                (Func<ValueT, float, ValueT>)mul, (Func<ValueT, float, ValueT>)div,
                (ValueT)zero);
    }

    public static void CacheOperators(Type valueType) => _ = GetOperatorsFromCache(valueType);

    internal static ValueT Add<ValueT>(in ValueT a, in ValueT b)
    {
        var (addition, _, _, _, _) = GetOperatorsFromCache(typeof(ValueT));
        return ((Func<ValueT, ValueT, ValueT>)addition).Invoke(a, b);
    }

    internal static ValueT Sub<ValueT>(in ValueT a, in ValueT b)
    {
        var (_, subtraction, _, _, _) = GetOperatorsFromCache(typeof(ValueT));
        return ((Func<ValueT, ValueT, ValueT>)subtraction).Invoke(a, b);
    }

    internal static ValueT Mul<ValueT>(in ValueT a, float k)
    {
        var (_, _, multiplication, _, _) = GetOperatorsFromCache(typeof(ValueT));
        return ((Func<ValueT, float, ValueT>)multiplication).Invoke(a, k);
    }

    internal static ValueT Div<ValueT>(in ValueT a, float k)
    {
        var (_, _, _, division, _) = GetOperatorsFromCache(typeof(ValueT));
        return ((Func<ValueT, float, ValueT>)division).Invoke(a, k);
    }

    internal static ValueT Zero<ValueT>()
    {
        var (_, _, _, _, zero) = GetOperatorsFromCache(typeof(ValueT));
        return (ValueT)zero;
    }
}
