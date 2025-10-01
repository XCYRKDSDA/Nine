using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Nine.Animations;

internal static class GenericMathHelper<ValueT>
{
    private delegate ValueT Addition(ValueT a, ValueT b);

    private delegate ValueT Subtraction(ValueT a, ValueT b);

    private delegate ValueT Multiplication(ValueT a, float k);

    private delegate ValueT Division(ValueT a, float k);

    private static readonly Addition _additionOperator;
    private static readonly Subtraction _subtractionOperator;
    private static readonly Multiplication _multiplicationOperator;
    private static readonly Division _divisionOperator;
    private static readonly ValueT _zero;

    static GenericMathHelper()
    {
        var valueType = typeof(ValueT);

        var expr_a = Expression.Parameter(valueType, "a");
        var expr_b = Expression.Parameter(valueType, "b");
        var expr_k = Expression.Parameter(typeof(float), "k");
        var expr_addition = Expression.Add(expr_a, expr_b);
        var expr_subtraction = Expression.Subtract(expr_a, expr_b);
        var expr_multiplication = Expression.Multiply(expr_a, expr_k);
        var expr_division = Expression.Divide(expr_a, expr_k);
        _additionOperator = Expression.Lambda<Addition>(expr_addition, expr_a, expr_b).Compile();
        _subtractionOperator = Expression.Lambda<Subtraction>(expr_subtraction, expr_a, expr_b).Compile();
        _multiplicationOperator = Expression.Lambda<Multiplication>(expr_multiplication, expr_a, expr_k).Compile();
        _divisionOperator = Expression.Lambda<Division>(expr_division, expr_a, expr_k).Compile();

        if (valueType.IsPrimitive)
            _zero = (ValueT)Convert.ChangeType(0, valueType);
        else if (valueType.GetField("Zero", BindingFlags.Static) is FieldInfo zeroField)
            _zero = (ValueT)zeroField.GetValue(null)!;
        else if (valueType.GetProperty("Zero", BindingFlags.Static) is PropertyInfo zeroProperty)
            _zero = (ValueT)zeroProperty.GetValue(null)!;
        else
        {
            var val = (ValueT)Activator.CreateInstance(valueType)!;
            _zero = _subtractionOperator.Invoke(val, val)!;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ValueT Add(in ValueT a, in ValueT b) => _additionOperator.Invoke(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ValueT Sub(in ValueT a, in ValueT b) => _subtractionOperator.Invoke(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ValueT Mul(in ValueT a, float k) => _multiplicationOperator.Invoke(a, k);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ValueT Div(in ValueT a, float k) => _divisionOperator.Invoke(a, k);

    internal static ValueT Zero => _zero;
}
